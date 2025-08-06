using ApiNetCore.Context;
using ApiNetCore.Controllers.Middleware;
using DotNetEnv;
using ApiNetCore.Services;
using ApiNetCore.Services.Interfaces;
using ApiNetCore.Mappings;
using Microsoft.EntityFrameworkCore;
// Cargar variables de entorno
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configuración...
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//DbContext MySql
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Servicios
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();  


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