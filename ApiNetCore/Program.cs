using ApiNetCore.ContextMysql;
using ApiNetCore.Controllers.Middleware;
using DotNetEnv;
using ApiNetCore.Mappings;
using ApiNetCore.Services;
using ApiNetCore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
// Cargar variables de entorno
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configuración...
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Servicios
builder.Services.AddScoped<IVendedorService, VendedorService>();


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