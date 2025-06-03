using System.Diagnostics;
using ApiNetCore.Dtos;
using ApiNetCore.Dtos.ProductTransactionReportDto;
using ApiNetCore.Dtos.TransactionDTOs;
using ApiNetCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCore.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TransactionDto>>>> GetAllAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        var transactions = await _transactionService.GetAllTransactionsAsync();
        stopwatch.Stop();

        var response = ApiResponse<IEnumerable<TransactionDto>>.SuccessResponse(
            transactions, 
            "Transactions retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> GetByIdAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        var transaction = await _transactionService.GetTransactionByIdAsync(id);
        stopwatch.Stop();

        var response = ApiResponse<TransactionDto>.SuccessResponse(
            transaction, 
            "Transaction retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TransactionDto>>>> GetByTypeAsync(string type)
    {
        var stopwatch = Stopwatch.StartNew();
        var transactions = await _transactionService.GetTransactionsByTypeAsync(type);
        stopwatch.Stop();

        var response = ApiResponse<IEnumerable<TransactionDto>>.SuccessResponse(
            transactions, 
            $"Transactions of type '{type}' retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TransactionDto>>>> GetByDateRangeAsync(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        var stopwatch = Stopwatch.StartNew();
        var transactions = await _transactionService.GetTransactionsByDateRangeAsync(startDate, endDate);
        stopwatch.Stop();

        var response = ApiResponse<IEnumerable<TransactionDto>>.SuccessResponse(
            transactions, 
            "Transactions retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> CreateAsync([FromBody] CreateTransactionDto createTransactionDto)
    {
        var stopwatch = Stopwatch.StartNew();
        var transaction = await _transactionService.CreateTransactionAsync(createTransactionDto);
        stopwatch.Stop();

        var response = ApiResponse<TransactionDto>.SuccessResponse(
            transaction, 
            "Transaction created successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> UpdateStatusAsync(int id, [FromBody] UpdateTransactionDto updateTransactionDto)
    {
        var stopwatch = Stopwatch.StartNew();
        var transaction = await _transactionService.UpdateTransactionStatusAsync(id, updateTransactionDto);
        stopwatch.Stop();

        var response = ApiResponse<TransactionDto>.SuccessResponse(
            transaction, 
            "Transaction updated successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<ApiResponse>> CancelAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        await _transactionService.CancelTransactionAsync(id);
        stopwatch.Stop();

        var response = ApiResponse.SuccessResponse("Transaction cancelled successfully");
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }
    
    [HttpPut("{id}/full")]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> UpdateTransactionAsync(int id, [FromBody] UpdateTransactionFullDto updateTransactionDto)
    {
        var stopwatch = Stopwatch.StartNew();
        var transaction = await _transactionService.UpdateTransactionAsync(id, updateTransactionDto);
        stopwatch.Stop();

        var response = ApiResponse<TransactionDto>.SuccessResponse(
            transaction, 
            "Transaction updated successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpPost("{id}/details")]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> AddDetailAsync(int id, [FromBody] AddDetailToTransactionDto addDetailDto)
    {
        var stopwatch = Stopwatch.StartNew();
        var transaction = await _transactionService.AddDetailToTransactionAsync(id, addDetailDto);
        stopwatch.Stop();

        var response = ApiResponse<TransactionDto>.SuccessResponse(
            transaction, 
            "Detail added to transaction successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpDelete("{transactionId}/details/{detailId}")]
    public async Task<ActionResult<ApiResponse>> RemoveDetailAsync(int transactionId, int detailId)
    {
        var stopwatch = Stopwatch.StartNew();
        await _transactionService.RemoveDetailFromTransactionAsync(transactionId, detailId);
        stopwatch.Stop();

        var response = ApiResponse.SuccessResponse("Detail removed from transaction successfully");
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }
    
     [HttpGet("{productId}/transactions")]
    public async Task<ActionResult<ApiResponse<ProductTransactionReportDto>>> GetProductTransactionReportAsync(int productId)
    {
        var stopwatch = Stopwatch.StartNew();
        var report = await _transactionService.GetProductTransactionReportAsync(productId);
        stopwatch.Stop();

        var response = ApiResponse<ProductTransactionReportDto>.SuccessResponse(
            report, 
            $"Transaction report for product ID {productId} retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpGet("code/{productCode}/transactions")]
    public async Task<ActionResult<ApiResponse<ProductTransactionReportDto>>> GetProductTransactionReportByCodeAsync(string productCode)
    {
        var stopwatch = Stopwatch.StartNew();
        var report = await _transactionService.GetProductTransactionReportByCodeAsync(productCode);
        stopwatch.Stop();

        var response = ApiResponse<ProductTransactionReportDto>.SuccessResponse(
            report, 
            $"Transaction report for product code '{productCode}' retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpPost("multiple/transactions")]
    public async Task<ActionResult<ApiResponse<MultipleProductsReportDto>>> GetMultipleProductsReportAsync([FromBody] List<int> productIds)
    {
        var stopwatch = Stopwatch.StartNew();
        var report = await _transactionService.GetMultipleProductsTransactionReportAsync(productIds);
        stopwatch.Stop();

        var response = ApiResponse<MultipleProductsReportDto>.SuccessResponse(
            report, 
            $"Transaction reports for {report.TotalProducts} products retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpGet("all/transactions")]
    public async Task<ActionResult<ApiResponse<MultipleProductsReportDto>>> GetAllProductsReportAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        var report = await _transactionService.GetAllProductsTransactionReportAsync();
        stopwatch.Stop();

        var response = ApiResponse<MultipleProductsReportDto>.SuccessResponse(
            report, 
            $"Transaction reports for all {report.TotalProducts} products retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    // Endpoint adicional para obtener solo el resumen de stock sin historial completo
    [HttpGet("{productId}/stock-summary")]
    public async Task<ActionResult<ApiResponse<ProductStockSummaryDto>>> GetProductStockSummaryAsync(int productId)
    {
        var stopwatch = Stopwatch.StartNew();
        var report = await _transactionService.GetProductTransactionReportAsync(productId);
        stopwatch.Stop();

        var response = ApiResponse<ProductStockSummaryDto>.SuccessResponse(
            report.StockSummary, 
            $"Stock summary for product ID {productId} retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }
    
}