using System.Diagnostics;
using ApiNetCore.Dtos;
using ApiNetCore.Entities;
using ApiNetCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiNetCore.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet ("categories")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Category>>>> GetAllCategoriesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        var categories = await _productService.GetAllCategoriesAsync();
        stopwatch.Stop();

        var response = ApiResponse<IEnumerable<Category>>.SuccessResponse(
            categories, 
            "Categories retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetAllAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        var products = await _productService.GetAllProductsAsync();
        stopwatch.Stop();

        var response = ApiResponse<IEnumerable<ProductDto>>.SuccessResponse(
            products, 
            "Products retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpGet("with-categories")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductWithCategoriesDto>>>> GetAllWithCategoriesAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        var products = await _productService.GetAllWithCategoriesAsync();
        stopwatch.Stop();

        var response = ApiResponse<IEnumerable<ProductWithCategoriesDto>>.SuccessResponse(
            products, 
            "Products with categories retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductWithCategoriesDto>>> GetByIdAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        var product = await _productService.GetByIdWithCategoriesAsync(id);
        stopwatch.Stop();

        var response = ApiResponse<ProductWithCategoriesDto>.SuccessResponse(
            product, 
            "Product retrieved successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductDto>>> CreateAsync([FromForm] CreateProductDto createProductDto)
    {
        var stopwatch = Stopwatch.StartNew();
        var product = await _productService.CreateProductAsync(createProductDto);
        stopwatch.Stop();

        var response = ApiResponse<ProductDto>.SuccessResponse(
            product, 
            "Product created successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> UpdateAsync(int id, [FromForm] UpdateProductDto updateProductDto)
    {
        var stopwatch = Stopwatch.StartNew();
        var product = await _productService.UpdateProductAsync(id, updateProductDto);
        stopwatch.Stop();

        var response = ApiResponse<ProductDto>.SuccessResponse(
            product, 
            "Product updated successfully"
        );
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteAsync(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        await _productService.DeleteAsync(id);
        stopwatch.Stop();

        var response = ApiResponse.SuccessResponse("Product deleted successfully");
        response.ResponseTimeMs = stopwatch.ElapsedMilliseconds;
        response.RequestId = HttpContext.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

        return Ok(response);
    }
    
    
}