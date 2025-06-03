using ApiNetCore.Dtos;
using ApiNetCore.Entities;

namespace ApiNetCore.Services.Interfaces;

public interface  IProductService : ICrudService<Product>
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<IEnumerable<ProductWithCategoriesDto>> GetAllWithCategoriesAsync();
    Task<ProductWithCategoriesDto> GetByIdWithCategoriesAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
    Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
    
}