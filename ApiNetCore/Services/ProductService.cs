using ApiNetCore.Context;
using ApiNetCore.Dtos;
using ApiNetCore.Entities;
using ApiNetCore.Exceptions;
using ApiNetCore.Services.Interfaces;
using ApiNetCore.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Services;

public class ProductService : IProductService
{
    private readonly ICloudinaryService _cloudinaryService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(ApplicationDbContext context, IMapper mapper, ICloudinaryService cloudinaryService)
    {
        _context = context;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.StatusCat == "A")
            .ToListAsync();
    }


    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    // Métodos con DTOs
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _context.Products.ToListAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductWithCategoriesDto>> GetAllWithCategoriesAsync()
    {
        var products = await _context.Products
            .Include(p => p.TblCategoryProducts)
            .ThenInclude(cp => cp.Cat)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ProductWithCategoriesDto>>(products);
    }

    public async Task<ProductWithCategoriesDto> GetByIdWithCategoriesAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.TblCategoryProducts)
            .ThenInclude(cp => cp.Cat)
            .FirstOrDefaultAsync(p => p.IdPro == id); // Usar IdPro, no Id

        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found");

        return _mapper.Map<ProductWithCategoriesDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        // Validar que el código no exista
        if (await _context.Products.AnyAsync(p => p.CodePro == createProductDto.Code))
            throw new ConflictException($"A product with code '{createProductDto.Code}' already exists");

        if (await _context.Products.AnyAsync(p => p.NamePro == createProductDto.Name))
            throw new ConflictException($"A product with name '{createProductDto.Name}' already exists");

        // Validar que las categorías existan
        await ValidateCategoriesExistAsync(createProductDto.CategoryIds);

        var product = _mapper.Map<Product>(createProductDto);

        // Generar código si no se proporciona
        if (string.IsNullOrEmpty(product.CodePro)) product.CodePro = await GenerateUniqueProductCodeAsync();

        // Subir imagen a Cloudinary si se proporciona
        if (createProductDto.Image != null)
            try
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(createProductDto.Image);
                product.ImagePro = imageUrl;
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Error uploading image: {ex.Message}", ex);
            }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Agregar relaciones con categorías
        if (createProductDto.CategoryIds.Count == 0) return _mapper.Map<ProductDto>(product);
        var categoryProducts = createProductDto.CategoryIds.Select(catId => new CategoryProduct
        {
            ProId = product.IdPro,
            CatId = catId,
            CreatedAt = DateTime.Now
        });

        _context.Set<CategoryProduct>().AddRange(categoryProducts);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.IdPro == id);
        if (existingProduct == null)
            throw new NotFoundException("Product", id);

        // Validar que el código no exista en otro producto
        if (!string.IsNullOrEmpty(updateProductDto.Code) &&
            await _context.Products.AnyAsync(p => p.CodePro == updateProductDto.Code && p.IdPro != id))
            throw new ConflictException($"A product with code '{updateProductDto.Code}' already exists");

        // Validar que las categorías existan
        await ValidateCategoriesExistAsync(updateProductDto.CategoryIds);

        // Mapear campos básicos
        existingProduct.NamePro = updateProductDto.Name;
        existingProduct.DescriptionPro = updateProductDto.Description;
        existingProduct.PriceUnitPro = updateProductDto.Price;
        existingProduct.StockPro = updateProductDto.Stock;
        existingProduct.UpdateAt = DateTime.Now;

        if (!string.IsNullOrEmpty(updateProductDto.Code))
            existingProduct.CodePro = updateProductDto.Code;

        // Manejar actualización de imagen
        if (updateProductDto.Image != null)
            try
            {
                var oldPublicId = CloudinaryHelper.ExtractPublicIdFromUrl(existingProduct.ImagePro);
                var newImageUrl = await _cloudinaryService.UpdateImageAsync(updateProductDto.Image, oldPublicId);
                existingProduct.ImagePro = newImageUrl;
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Error updating image: {ex.Message}", ex);
            }

        // Actualizar relaciones con categorías
        if (updateProductDto.CategoryIds.Count != 0)
        {
            var existingRelations = await _context.Set<CategoryProduct>()
                .Where(cp => cp.ProId == id)
                .ToListAsync();
            _context.Set<CategoryProduct>().RemoveRange(existingRelations);

            var newCategoryProducts = updateProductDto.CategoryIds.Select(catId => new CategoryProduct
            {
                ProId = id,
                CatId = catId,
                CreatedAt = DateTime.Now
            });
            _context.Set<CategoryProduct>().AddRange(newCategoryProducts);
        }

        await _context.SaveChangesAsync();
        return _mapper.Map<ProductDto>(existingProduct);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.IdPro == id);
        if (product == null)
            throw new NotFoundException("Product", id);

        // Eliminar imagen de Cloudinary si existe
        if (!string.IsNullOrEmpty(product.ImagePro))
        {
            var publicId = CloudinaryHelper.ExtractPublicIdFromUrl(product.ImagePro);
            if (!string.IsNullOrEmpty(publicId)) await _cloudinaryService.DeleteImageAsync(publicId);
        }
        
        // Eliminar relaciones con categorías
        var categoryProducts = await _context.Set<CategoryProduct>().Where(cp => cp.ProId == id).ToListAsync();
        _context.Set<CategoryProduct>().RemoveRange(categoryProducts);
        
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        return product ?? throw new KeyNotFoundException($"Product with ID {id} not found");
    }

    public async Task<Product> CreateAsync(Product entity)
    {
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Product> UpdateAsync(int id, Product entity)
    {
        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
            throw new KeyNotFoundException($"Product with ID {id} not found");

        _context.Entry(existingProduct).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return existingProduct;
    }

    private async Task ValidateCategoriesExistAsync(List<int> categoryIds)
    {
        if (!categoryIds.Any()) return;

        var existingCategoryIds = await _context.Set<Category>()
            .Where(c => categoryIds.Contains(c.IdCat))
            .Select(c => c.IdCat)
            .ToListAsync();

        var nonExistentCategories = categoryIds.Except(existingCategoryIds).ToList();

        if (nonExistentCategories.Count != 0)
        {
            var errors = new Dictionary<string, List<string>>
            {
                {
                    "CategoryIds",
                    [$"The following category IDs do not exist: {string.Join(", ", nonExistentCategories)}"]
                }
            };
            throw new ValidationException(errors);
        }
    }

    private async Task<string> GenerateUniqueProductCodeAsync()
    {
        string code;
        do
        {
            code = $"PRO-{DateTime.Now:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
        } while (await _context.Products.AnyAsync(p => p.CodePro == code));

        return code;
    }
}