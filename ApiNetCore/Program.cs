using ApiNetCore.ContextMysql;
using ApiNetCore.Controllers.Middleware;
using DotNetEnv;
using ApiNetCore.Mappings;
using ApiNetCore.Services;
using ApiNetCore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

// Cargar variables de entorno
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configuración...
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Servicios
builder.Services.AddScoped<IGeocercaService, GeocercaService>();
builder.Services.AddScoped<IVendedorExternoService, VendedorExternoService>();




builder.Services.AddHttpClient<IVendedorExternoService, VendedorExternoService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "ApiNetCore/1.0");
});





//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});



// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


//DbContext MySql
builder.Services.AddDbContext<MyDbContextMysql>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnectionMysql"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnectionMysql"))));


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Geolocalización",
        Version = "v1",
        Description = "API para integración con el sistema de vendedores y geolocalizaciónr",
        Contact = new OpenApiContact
        {
            Name = "Soporte",
            Email = "soporte@empresa.com"
        }
    });
});




var app = builder.Build();

// Middlewares (orden importante)
app.UseMiddleware<ResponseTimeMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors();

app.Run();