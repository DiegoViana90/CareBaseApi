using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CareBaseApi.Data; // seu DbContext aqui

var builder = WebApplication.CreateBuilder(args);

// Configurar o DbContext com PostgreSQL (ajuste a connection string conforme seu ambiente)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar controllers
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CareBase API", Version = "v1" });
    // Aqui você pode configurar autenticação JWT se quiser futuramente
});

var app = builder.Build();

// Pipeline HTTP

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CareBase API V1");
        c.RoutePrefix = string.Empty; // Swagger na raiz http://localhost:5000/
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
