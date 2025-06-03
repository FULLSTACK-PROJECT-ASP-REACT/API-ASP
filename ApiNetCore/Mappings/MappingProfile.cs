using ApiNetCore.Dtos;
using ApiNetCore.Dtos.TransactionDTOs;
using ApiNetCore.Entities;
using AutoMapper;

namespace ApiNetCore.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdPro))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NamePro))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionPro))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceUnitPro))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusPro))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockPro))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CodePro))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImagePro));

        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.NamePro, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DescriptionPro, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.PriceUnitPro, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.StatusPro, opt => opt.MapFrom(src => "A")) // Activo por defecto
            .ForMember(dest => dest.StockPro, opt => opt.MapFrom(src => src.Stock)) // Stock inicial
            .ForMember(dest => dest.TblCategoryProducts, opt => opt.Ignore())
            .ForMember(dest => dest.IdPro, opt => opt.Ignore())
            .ForMember(dest => dest.CodePro, opt => opt.Ignore()) // Lo puedes generar automáticamente
            .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
            .ForMember(dest => dest.ImagePro, opt => opt.Ignore())
            .ForMember(dest => dest.TblDetailTransactions, opt => opt.Ignore());

        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.NamePro, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DescriptionPro, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.PriceUnitPro, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.TblCategoryProducts, opt => opt.Ignore())
            .ForMember(dest => dest.IdPro, opt => opt.Ignore())
            .ForMember(dest => dest.CodePro, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.StatusPro, opt => opt.Ignore())
            .ForMember(dest => dest.StockPro, opt => opt.Ignore())
            .ForMember(dest => dest.ImagePro, opt => opt.Ignore())
            .ForMember(dest => dest.TblDetailTransactions, opt => opt.Ignore());

        // Product with Categories mapping
        CreateMap<Product, ProductWithCategoriesDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdPro))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NamePro))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionPro))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockPro))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusPro))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CodePro))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImagePro))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceUnitPro))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                src.TblCategoryProducts
                    .Where(cp => cp.Cat != null)
                    .Select(cp => cp.Cat)));

        // Category mappings con nombres correctos
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdCat))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NameCat))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusCat));
        //
        // Transaction mappings con nombres correctos
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdTra))
            .ForMember(dest => dest.EmissionDate, opt => opt.MapFrom(src => src.EmissionDateTra))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeTra))
            .ForMember(dest => dest.TypeDescription, opt => opt.MapFrom(src =>
                src.TypeTra == "C" ? "Compra" : src.TypeTra == "V" ? "Venta" : "Desconocido"))
            .ForMember(dest => dest.PriceUnit, opt => opt.MapFrom(src => src.PriceUnitTra))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmountTra))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusTra))
            .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src =>
                src.StatusTra == "A" ? "Activo" :
                src.StatusTra == "I" ? "Inactivo" :
                src.StatusTra == "C" ? "Cancelado" : "Desconocido"))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.MessageTra))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.TblDetailTransactions));

        CreateMap<CreateTransactionDto, Transaction>()
            .ForMember(dest => dest.TypeTra, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.EmissionDateTra, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.StatusTra, opt => opt.MapFrom(src => "A")) // Activo por defecto
            .ForMember(dest => dest.MessageTra, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.IdTra, opt => opt.Ignore())
            .ForMember(dest => dest.PriceUnitTra, opt => opt.Ignore()) // Se calculará
            .ForMember(dest => dest.TotalAmountTra, opt => opt.Ignore()) // Se calculará
            .ForMember(dest => dest.TblDetailTransactions, opt => opt.Ignore());

        // DetailTransaction mappings
        CreateMap<DetailTransaction, DetailTransactionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdDT))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Pro != null ? src.Pro.NamePro : null))
            .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.Pro != null ? src.Pro.CodePro : null))
            .ForMember(dest => dest.CodeStub, opt => opt.MapFrom(src => src.CodeStub))
            .ForMember(dest => dest.PriceUnit, opt => opt.MapFrom(src => src.PriceUnit))
            .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Subtotal))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionTra));

        CreateMap<CreateDetailTransactionDto, DetailTransaction>()
            .ForMember(dest => dest.ProId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.DescriptionTra, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IdDT, opt => opt.Ignore())
            .ForMember(dest => dest.TraId, opt => opt.Ignore())
            .ForMember(dest => dest.CodeStub, opt => opt.Ignore())
            .ForMember(dest => dest.PriceUnit, opt => opt.Ignore())
            .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
            .ForMember(dest => dest.Total, opt => opt.Ignore())
            .ForMember(dest => dest.Pro, opt => opt.Ignore())
            .ForMember(dest => dest.Tra, opt => opt.Ignore());
    }
}