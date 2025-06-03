using ApiNetCore.Context;
using ApiNetCore.Dtos.ProductTransactionReportDto;
using ApiNetCore.Dtos.TransactionDTOs;
using ApiNetCore.Entities;
using ApiNetCore.Enum;
using ApiNetCore.Exceptions;
using ApiNetCore.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Services;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TransactionService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
    {
        var transactions = await _context.Set<Transaction>()
            .Include(t => t.TblDetailTransactions)
            .ThenInclude(dt => dt.Pro)
            .OrderByDescending(t => t.EmissionDateTra)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<TransactionDto> GetTransactionByIdAsync(int id)
    {
        var transaction = await _context.Set<Transaction>()
            .Include(t => t.TblDetailTransactions)
            .ThenInclude(dt => dt.Pro)
            .FirstOrDefaultAsync(t => t.IdTra == id);

        if (transaction == null)
            throw new NotFoundException("Transaction", id);

        return _mapper.Map<TransactionDto>(transaction);
    }

    public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto)
    {
        // Validar tipo de transacción
        if (!TransactionExtensions.IsValidTransactionType(createTransactionDto.Type))
            throw new BadRequestException("Invalid transaction type. Use 'C' for Purchase or 'V' for Sale");

        // Validar que los productos existan y calcular totales
        await ValidateProductsAndCalculateTotalsAsync(createTransactionDto);

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Crear la transacción principal
            var newTransaction = _mapper.Map<Transaction>(createTransactionDto);

            // Generar código único para la transacción
            newTransaction.CodeStub = await GenerateTransactionCodeAsync(createTransactionDto.Type);

            _context.Set<Transaction>().Add(newTransaction);
            await _context.SaveChangesAsync();

            // Crear los detalles de transacción y actualizar stock
            var totalTransactionAmount = 0;
            var details = new List<DetailTransaction>();

            foreach (var detailDto in createTransactionDto.Details)
            {
                var product = await _context.Products.FirstAsync(p => p.IdPro == detailDto.ProductId);

                // Validar stock para ventas
                if (createTransactionDto.Type == "V" && product.StockPro < detailDto.Amount)
                    throw new BadRequestException(
                        $"Insufficient stock for product '{product.NamePro}'. Available: {product.StockPro}, Requested: {detailDto.Amount}");

                var detail = new DetailTransaction
                {
                    TraId = newTransaction.IdTra,
                    ProId = detailDto.ProductId,
                    Amount = detailDto.Amount,
                    PriceUnit = detailDto.CustomPriceUnit ?? product.PriceUnitPro,
                    DescriptionTra = detailDto.Description,
                    CodeStub = $"{newTransaction.CodeStub}-{details.Count + 1:D3}"
                };

                // Calcular subtotal y total
                detail.Subtotal = detail.PriceUnit * detail.Amount;
                detail.Total = detail.Subtotal; // Aquí podrías agregar impuestos, descuentos, etc.

                details.Add(detail);
                totalTransactionAmount += (int)detail.Total;

                // Actualizar stock según el tipo de transacción
                if (createTransactionDto.Type == "V") // Venta - resta stock
                    product.StockPro -= detailDto.Amount;
                else if (createTransactionDto.Type == "C") // Compra - suma stock
                    product.StockPro += detailDto.Amount;

                product.UpdateAt = DateTime.Now;
            }

            // Actualizar totales de la transacción
            newTransaction.TotalAmountTra = totalTransactionAmount;
            newTransaction.PriceUnitTra = totalTransactionAmount; // En este caso es lo mismo

            _context.Set<DetailTransaction>().AddRange(details);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            // Cargar la transacción completa para devolver
            var createdTransaction = await GetTransactionByIdAsync(newTransaction.IdTra);
            return createdTransaction;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<TransactionDto> UpdateTransactionStatusAsync(int id, UpdateTransactionDto updateTransactionDto)
    {
        var transaction = await _context.Set<Transaction>().FirstOrDefaultAsync(t => t.IdTra == id);
        if (transaction == null)
            throw new NotFoundException("Transaction", id);

        // Validar estado si se proporciona
        if (!string.IsNullOrEmpty(updateTransactionDto.Status) &&
            !TransactionExtensions.IsValidTransactionStatus(updateTransactionDto.Status))
            throw new BadRequestException(
                "Invalid transaction status. Use 'A' for Active, 'I' for Inactive, or 'C' for Cancelled");

        // No permitir modificar transacciones canceladas
        if (transaction.StatusTra == "C")
            throw new BadRequestException("Cannot modify a cancelled transaction");

        if (!string.IsNullOrEmpty(updateTransactionDto.Status))
            transaction.StatusTra = updateTransactionDto.Status;

        if (!string.IsNullOrEmpty(updateTransactionDto.Message))
            transaction.MessageTra = updateTransactionDto.Message;

        await _context.SaveChangesAsync();

        return await GetTransactionByIdAsync(id);
    }

    public async Task<bool> CancelTransactionAsync(int id)
    {
        var transaction = await _context.Set<Transaction>()
            .Include(t => t.TblDetailTransactions)
            .ThenInclude(dt => dt.Pro)
            .FirstOrDefaultAsync(t => t.IdTra == id);

        if (transaction == null)
            throw new NotFoundException("Transaction", id);

        if (transaction.StatusTra == "C")
            throw new BadRequestException("Transaction is already cancelled");

        await using var dbTransaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Revertir cambios de stock
            foreach (var detail in transaction.TblDetailTransactions)
                if (detail.Pro != null)
                {
                    if (transaction.TypeTra == "V") // Si fue venta, devolver stock
                    {
                        detail.Pro.StockPro += detail.Amount;
                    }
                    else if (transaction.TypeTra == "C") // Si fue compra, restar stock
                    {
                        detail.Pro.StockPro -= detail.Amount;

                        // Validar que no quede stock negativo
                        if (detail.Pro.StockPro < 0)
                            throw new BadRequestException(
                                $"Cannot cancel transaction: would result in negative stock for product '{detail.Pro.NamePro}'");
                    }

                    detail.Pro.UpdateAt = DateTime.Now;
                }

            // Marcar transacción como cancelada
            transaction.StatusTra = "C";
            transaction.MessageTra = $"Transaction cancelled on {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

            await _context.SaveChangesAsync();
            await dbTransaction.CommitAsync();

            return true;
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByTypeAsync(string type)
    {
        if (!TransactionExtensions.IsValidTransactionType(type))
            throw new BadRequestException("Invalid transaction type. Use 'C' for Purchase or 'V' for Sale");

        var transactions = await _context.Set<Transaction>()
            .Include(t => t.TblDetailTransactions)
            .ThenInclude(dt => dt.Pro)
            .Where(t => t.TypeTra == type)
            .OrderByDescending(t => t.EmissionDateTra)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new BadRequestException("Start date cannot be greater than end date");

        var transactions = await _context.Set<Transaction>()
            .Include(t => t.TblDetailTransactions)
            .ThenInclude(dt => dt.Pro)
            .Where(t => t.EmissionDateTra >= startDate && t.EmissionDateTra <= endDate)
            .OrderByDescending(t => t.EmissionDateTra)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<TransactionDto> UpdateTransactionAsync(int id, UpdateTransactionFullDto updateTransactionDto)
    {
        var transaction = await _context.Set<Transaction>()
            .Include(t => t.TblDetailTransactions)
            .ThenInclude(dt => dt.Pro)
            .FirstOrDefaultAsync(t => t.IdTra == id);

        if (transaction == null)
            throw new NotFoundException("Transaction", id);

        // No permitir modificar transacciones canceladas
        if (transaction.StatusTra == "C")
            throw new BadRequestException("Cannot modify a cancelled transaction");

        // Validar estado si se proporciona
        if (!string.IsNullOrEmpty(updateTransactionDto.Status) &&
            !TransactionExtensions.IsValidTransactionStatus(updateTransactionDto.Status))
            throw new BadRequestException(
                "Invalid transaction status. Use 'A' for Active, 'I' for Inactive, or 'C' for Cancelled");

        using var dbTransaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Actualizar campos básicos de la transacción
            if (!string.IsNullOrEmpty(updateTransactionDto.Status))
                transaction.StatusTra = updateTransactionDto.Status;

            if (!string.IsNullOrEmpty(updateTransactionDto.Message))
                transaction.MessageTra = updateTransactionDto.Message;

            // Procesar actualizaciones de detalles
            var totalTransactionAmount = 0m;

            foreach (var detailUpdate in updateTransactionDto.Details)
            {
                var existingDetail = transaction.TblDetailTransactions
                    .FirstOrDefault(d => d.IdDT == detailUpdate.DetailId);

                if (existingDetail == null)
                    throw new NotFoundException("Transaction detail", detailUpdate.DetailId);

                // Guardar valores anteriores para poder revertir stock
                var previousAmount = existingDetail.Amount;
                var previousProductId = existingDetail.ProId;

                // Revertir stock del estado anterior
                if (previousProductId.HasValue)
                {
                    var previousProduct = await _context.Products.FirstAsync(p => p.IdPro == previousProductId.Value);

                    if (transaction.TypeTra == "V") // Si es venta, devolver stock anterior
                        previousProduct.StockPro += previousAmount;
                    else if (transaction.TypeTra == "C") // Si es compra, restar stock anterior
                        previousProduct.StockPro -= previousAmount;
                }

                // Determinar el producto a usar (nuevo o existente)
                var productId = detailUpdate.NewProductId ?? existingDetail.ProId!.Value;
                var product = await _context.Products.FirstOrDefaultAsync(p => p.IdPro == productId);

                if (product == null)
                    throw new NotFoundException("Product", productId);

                // Validar stock para el nuevo estado
                if (transaction.TypeTra == "V") // Venta
                {
                    if (product.StockPro < detailUpdate.Amount)
                        throw new BadRequestException(
                            $"Insufficient stock for product '{product.NamePro}'. Available: {product.StockPro}, Requested: {detailUpdate.Amount}");

                    // Aplicar nuevo stock
                    product.StockPro -= detailUpdate.Amount;
                }
                else if (transaction.TypeTra == "C") // Compra
                {
                    // Aplicar nuevo stock
                    product.StockPro += detailUpdate.Amount;
                }

                product.UpdateAt = DateTime.Now;

                // Actualizar el detalle
                existingDetail.ProId = productId;
                existingDetail.Amount = detailUpdate.Amount;
                existingDetail.PriceUnit = detailUpdate.PriceUnit;
                existingDetail.DescriptionTra = detailUpdate.Description;

                // Recalcular subtotal y total
                existingDetail.Subtotal = existingDetail.PriceUnit * existingDetail.Amount;
                existingDetail.Total = existingDetail.Subtotal; // Aquí podrías agregar impuestos

                totalTransactionAmount += existingDetail.Total;
            }

            // Actualizar totales de la transacción
            transaction.TotalAmountTra = (int)totalTransactionAmount;
            transaction.PriceUnitTra = (int)totalTransactionAmount;

            await _context.SaveChangesAsync();
            await dbTransaction.CommitAsync();

            return await GetTransactionByIdAsync(id);
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task<TransactionDto> AddDetailToTransactionAsync(int transactionId,
        AddDetailToTransactionDto addDetailDto)
    {
        var transaction = await _context.Set<Transaction>()
            .Include(t => t.TblDetailTransactions)
            .FirstOrDefaultAsync(t => t.IdTra == transactionId);

        if (transaction == null)
            throw new NotFoundException("Transaction", transactionId);

        if (transaction.StatusTra == "C")
            throw new BadRequestException("Cannot modify a cancelled transaction");

        var product = await _context.Products.FirstOrDefaultAsync(p => p.IdPro == addDetailDto.ProductId);
        if (product == null)
            throw new NotFoundException("Product", addDetailDto.ProductId);

        using var dbTransaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Validar stock para ventas
            if (transaction.TypeTra == "V" && product.StockPro < addDetailDto.Amount)
                throw new BadRequestException(
                    $"Insufficient stock for product '{product.NamePro}'. Available: {product.StockPro}, Requested: {addDetailDto.Amount}");

            // Crear nuevo detalle
            var newDetail = new DetailTransaction
            {
                TraId = transactionId,
                ProId = addDetailDto.ProductId,
                Amount = addDetailDto.Amount,
                PriceUnit = addDetailDto.CustomPriceUnit ?? product.PriceUnitPro,
                DescriptionTra = addDetailDto.Description,
                CodeStub = $"{transaction.CodeStub}-{transaction.TblDetailTransactions.Count + 1:D3}"
            };

            newDetail.Subtotal = newDetail.PriceUnit * newDetail.Amount;
            newDetail.Total = newDetail.Subtotal;

            // Actualizar stock
            if (transaction.TypeTra == "V") // Venta
                product.StockPro -= addDetailDto.Amount;
            else if (transaction.TypeTra == "C") // Compra
                product.StockPro += addDetailDto.Amount;

            product.UpdateAt = DateTime.Now;

            // Agregar detalle
            _context.Set<DetailTransaction>().Add(newDetail);

            // Recalcular totales de la transacción
            var newTotal = transaction.TblDetailTransactions.Sum(d => d.Total) + newDetail.Total;
            transaction.TotalAmountTra = (int)newTotal;
            transaction.PriceUnitTra = (int)newTotal;

            await _context.SaveChangesAsync();
            await dbTransaction.CommitAsync();

            return await GetTransactionByIdAsync(transactionId);
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> RemoveDetailFromTransactionAsync(int transactionId, int detailId)
    {
        var transaction = await _context.Set<Transaction>()
            .Include(t => t.TblDetailTransactions)
            .ThenInclude(dt => dt.Pro)
            .FirstOrDefaultAsync(t => t.IdTra == transactionId);

        if (transaction == null)
            throw new NotFoundException("Transaction", transactionId);

        if (transaction.StatusTra == "C")
            throw new BadRequestException("Cannot modify a cancelled transaction");

        var detailToRemove = transaction.TblDetailTransactions.FirstOrDefault(d => d.IdDT == detailId);
        if (detailToRemove == null)
            throw new NotFoundException("Transaction detail", detailId);

        // Validar que no sea el último detalle
        if (transaction.TblDetailTransactions.Count <= 1)
            throw new BadRequestException("Cannot remove the last detail from a transaction");

        await using var dbTransaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Revertir stock
            if (detailToRemove is { ProId: not null, Pro: not null })
            {
                switch (transaction.TypeTra)
                {
                    // Si es venta, devolver stock
                    case "V":
                        detailToRemove.Pro.StockPro += detailToRemove.Amount;
                        break;
                    // Si es compra, restar stock
                    case "C":
                    {
                        detailToRemove.Pro.StockPro -= detailToRemove.Amount;

                        if (detailToRemove.Pro.StockPro < 0)
                            throw new BadRequestException(
                                $"Cannot remove detail: would result in negative stock for product '{detailToRemove.Pro.NamePro}'");
                        break;
                    }
                }

                detailToRemove.Pro.UpdateAt = DateTime.Now;
            }

            // Remover detalle
            _context.Set<DetailTransaction>().Remove(detailToRemove);

            // Recalcular totales de la transacción
            var newTotal = transaction.TblDetailTransactions
                .Where(d => d.IdDT != detailId)
                .Sum(d => d.Total);

            transaction.TotalAmountTra = (int)newTotal;
            transaction.PriceUnitTra = (int)newTotal;

            await _context.SaveChangesAsync();
            await dbTransaction.CommitAsync();

            return true;
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ProductTransactionReportDto> GetProductTransactionReportAsync(int productId)
    {
        // Verificar que el producto existe
        var product = await _context.Products.FirstOrDefaultAsync(p => p.IdPro == productId);
        if (product == null)
            throw new NotFoundException("Product", productId);

        // Obtener todas las transacciones de este producto
        var transactionDetails = await _context.Set<DetailTransaction>()
            .Include(dt => dt.Tra)
            .Include(dt => dt.Pro)
            .Where(dt => dt.ProId == productId)
            .OrderByDescending(dt => dt.Tra!.EmissionDateTra)
            .ToListAsync();

        // Mapear información del producto
        var productSummary = new ProductSummaryDto
        {
            Id = product.IdPro,
            Code = product.CodePro,
            Name = product.NamePro,
            Description = product.DescriptionPro,
            CurrentPrice = product.PriceUnitPro,
            CurrentStock = product.StockPro,
            Status = product.StatusPro,
            Image = product.ImagePro,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdateAt
        };

        // Construir historial de transacciones
        var transactionHistory = new List<ProductTransactionHistoryDto>();
        var runningStock = product.StockPro; // Stock actual

        // Calcular stock histórico (empezando desde el más reciente hacia atrás)
        for (var i = 0; i < transactionDetails.Count; i++)
        {
            var detail = transactionDetails[i];
            var transaction = detail.Tra!;

            var stockChange = 0;
            var stockImpact = "";
            var stockAfterTransaction = runningStock;

            if (transaction.TypeTra == "V") // Venta
            {
                stockChange = detail.Amount;
                stockImpact = "-";
                // Para calcular el stock antes de esta transacción (hacia atrás)
                runningStock += detail.Amount;
            }
            else if (transaction.TypeTra == "C") // Compra
            {
                stockChange = detail.Amount;
                stockImpact = "+";
                // Para calcular el stock antes de esta transacción (hacia atrás)
                runningStock -= detail.Amount;
            }

            var historyItem = new ProductTransactionHistoryDto
            {
                TransactionId = transaction.IdTra,
                TransactionCode = transaction.CodeStub ?? $"TXN-{transaction.IdTra}",
                TransactionDate = transaction.EmissionDateTra,
                TransactionType = transaction.TypeTra,
                TransactionTypeDescription = transaction.TypeTra == "C" ? "Compra" :
                    transaction.TypeTra == "V" ? "Venta" : "Desconocido",
                TransactionStatus = transaction.StatusTra,
                TransactionStatusDescription = transaction.StatusTra == "A" ? "Activo" :
                    transaction.StatusTra == "I" ? "Inactivo" :
                    transaction.StatusTra == "C" ? "Cancelado" : "Desconocido",
                TransactionMessage = transaction.MessageTra,
                DetailId = detail.IdDT,
                Quantity = detail.Amount,
                UnitPrice = detail.PriceUnit,
                Subtotal = detail.Subtotal,
                Total = detail.Total,
                DetailDescription = detail.DescriptionTra,
                DetailCode = detail.CodeStub,
                StockImpact = stockImpact,
                StockChange = stockChange,
                StockAfterTransaction = stockAfterTransaction
            };

            transactionHistory.Add(historyItem);
        }

        // Invertir la lista para mostrar cronológicamente (más antigua primero)
        transactionHistory.Reverse();

        // Corregir el cálculo de stock after transaction para orden cronológico
        var currentCalculatedStock = runningStock; // Stock inicial calculado
        foreach (var item in transactionHistory)
        {
            if (item.StockImpact == "+")
                currentCalculatedStock += item.StockChange;
            else if (item.StockImpact == "-") currentCalculatedStock -= item.StockChange;
            item.StockAfterTransaction = currentCalculatedStock;
        }

        // Calcular resumen de stock
        var stockSummary = CalculateStockSummary(transactionDetails, product.StockPro);

        return new ProductTransactionReportDto
        {
            Product = productSummary,
            TransactionHistory = transactionHistory,
            StockSummary = stockSummary
        };
    }

    public async Task<ProductTransactionReportDto> GetProductTransactionReportByCodeAsync(string productCode)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.CodePro == productCode);
        if (product == null)
            throw new NotFoundException($"Product with code '{productCode}' not found");

        return await GetProductTransactionReportAsync(product.IdPro);
    }

    public async Task<MultipleProductsReportDto> GetMultipleProductsTransactionReportAsync(List<int> productIds)
    {
        if (!productIds.Any())
            throw new BadRequestException("At least one product ID is required");

        var reports = new List<ProductTransactionReportDto>();

        foreach (var productId in productIds.Distinct())
            try
            {
                var report = await GetProductTransactionReportAsync(productId);
                reports.Add(report);
            }
            catch (NotFoundException)
            {
                // Ignorar productos que no existen, o podrías agregar un log aquí
            }

        return new MultipleProductsReportDto
        {
            Products = reports,
            TotalProducts = reports.Count,
            ReportGeneratedAt = DateTime.Now
        };
    }

    public async Task<MultipleProductsReportDto> GetAllProductsTransactionReportAsync()
    {
        var allProductIds = await _context.Products.Select(p => p.IdPro).ToListAsync();
        return await GetMultipleProductsTransactionReportAsync(allProductIds);
    }

    private async Task ValidateProductsAndCalculateTotalsAsync(CreateTransactionDto createTransactionDto)
    {
        var productIds = createTransactionDto.Details.Select(d => d.ProductId).Distinct().ToList();

        var existingProducts = await _context.Products
            .Where(p => productIds.Contains(p.IdPro))
            .Select(p => p.IdPro)
            .ToListAsync();

        var nonExistentProducts = productIds.Except(existingProducts).ToList();

        if (nonExistentProducts.Any())
        {
            var errors = new Dictionary<string, List<string>>
            {
                {
                    "ProductIds",
                    [$"The following product IDs do not exist: {string.Join(", ", nonExistentProducts)}"]
                }
            };
            throw new ValidationException(errors);
        }
    }

    private async Task<string> GenerateTransactionCodeAsync(string type)
    {
        var prefix = type == "V" ? "VT" : "CT"; // VT = Venta Transaction, CT = Compra Transaction
        var date = DateTime.Now.ToString("yyyyMMdd");

        var lastTransaction = await _context.Set<Transaction>()
            .Where(t => t.TypeTra == type)
            .OrderByDescending(t => t.IdTra)
            .FirstOrDefaultAsync();

        var sequence = 1;
        if (lastTransaction != null)
        {
            // Extraer el número de secuencia del último código si es posible
            // Formato esperado: VT-20250603-0001 o CT-20250603-0001
            var parts = lastTransaction.CodeStub?.Split('-');
            if (parts?.Length == 3 && int.TryParse(parts[2], out var lastSequence))
                sequence = lastSequence + 1;
            else
                // Si no se puede parsear, usar el ID + 1
                sequence = lastTransaction.IdTra + 1;
        }

        return $"{prefix}-{date}-{sequence:D4}";
    }

    private ProductStockSummaryDto CalculateStockSummary(List<DetailTransaction> transactionDetails, int currentStock)
    {
        var purchaseDetails =
            transactionDetails.Where(dt => dt.Tra!.TypeTra == "C" && dt.Tra.StatusTra == "A").ToList();
        var saleDetails = transactionDetails.Where(dt => dt.Tra!.TypeTra == "V" && dt.Tra.StatusTra == "A").ToList();

        var totalPurchased = purchaseDetails.Sum(dt => dt.Amount);
        var totalSold = saleDetails.Sum(dt => dt.Amount);
        var totalPurchaseValue = purchaseDetails.Sum(dt => dt.Total);
        var totalSaleValue = saleDetails.Sum(dt => dt.Total);

        var averagePurchasePrice = purchaseDetails.Any() ? totalPurchaseValue / totalPurchased : 0;
        var averageSalePrice = saleDetails.Any() ? totalSaleValue / totalSold : 0;

        var allTransactionDates = transactionDetails
            .Where(dt => dt.Tra?.EmissionDateTra != null)
            .Select(dt => dt.Tra!.EmissionDateTra!.Value)
            .ToList();

        return new ProductStockSummaryDto
        {
            CurrentStock = currentStock,
            TotalPurchased = totalPurchased,
            TotalSold = totalSold,
            TotalPurchaseValue = totalPurchaseValue,
            TotalSaleValue = totalSaleValue,
            AveragePurchasePrice = averagePurchasePrice,
            AverageSalePrice = averageSalePrice,
            TotalTransactions = transactionDetails.Count,
            FirstTransactionDate = allTransactionDates.Any() ? allTransactionDates.Min() : null,
            LastTransactionDate = allTransactionDates.Any() ? allTransactionDates.Max() : null
        };
    }
}