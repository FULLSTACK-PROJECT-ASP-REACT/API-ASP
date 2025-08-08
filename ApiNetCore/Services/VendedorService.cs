using ApiNetCore.ContextMysql;
using ApiNetCore.Dtos.Geocerca;
using ApiNetCore.Entities;
using ApiNetCore.Exceptions;
using ApiNetCore.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Services;

public class VendedorService(MyDbContextMysql dbContextMysql, IMapper mapper) : IVendedorService
{
    public async Task<CreateVendedorDto> CreateVendedor(CreateVendedorDto createVendedorDto)
    {
        if (await dbContextMysql.TblGeocercaVendedors.AnyAsync(x => x.CodeVendedor == createVendedorDto.CodeVendedor))
            throw new ConflictException($"El codigo {createVendedorDto.CodeVendedor} ya existe en la base de datos");
        if (await dbContextMysql.TblGeocercaVendedors.AnyAsync(x => x.NombreVendedor == createVendedorDto.NombreVendedor))
            throw new ConflictException($"El nombre {createVendedorDto.NombreVendedor} ya existe en la base de datos");
        
        var newVendedor = mapper.Map<CreateVendedorDto, GeocercaVendedor>(createVendedorDto);
        await dbContextMysql.TblGeocercaVendedors.AddAsync(newVendedor);
        await dbContextMysql.SaveChangesAsync();
        return createVendedorDto;
    }
}