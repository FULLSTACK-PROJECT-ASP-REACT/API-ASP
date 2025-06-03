using ApiNetCore.Dtos.ProductTransactionReportDto;
using ApiNetCore.Dtos.TransactionDTOs;

namespace ApiNetCore.Services.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
    Task<TransactionDto> GetTransactionByIdAsync(int id);
    Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createTransactionDto);
    Task<TransactionDto> UpdateTransactionStatusAsync(int id, UpdateTransactionDto updateTransactionDto);
    Task<bool> CancelTransactionAsync(int id);
    Task<IEnumerable<TransactionDto>> GetTransactionsByTypeAsync(string type);
    Task<IEnumerable<TransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<TransactionDto> UpdateTransactionAsync(int id, UpdateTransactionFullDto updateTransactionDto);
    Task<TransactionDto> AddDetailToTransactionAsync(int transactionId, AddDetailToTransactionDto addDetailDto);
    Task<bool> RemoveDetailFromTransactionAsync(int transactionId, int detailId);
    
    // Agregar estos métodos a la interfaz existente
    Task<ProductTransactionReportDto> GetProductTransactionReportAsync(int productId);
    Task<MultipleProductsReportDto> GetMultipleProductsTransactionReportAsync(List<int> productIds);
    Task<MultipleProductsReportDto> GetAllProductsTransactionReportAsync();
    Task<ProductTransactionReportDto> GetProductTransactionReportByCodeAsync(string productCode);
}