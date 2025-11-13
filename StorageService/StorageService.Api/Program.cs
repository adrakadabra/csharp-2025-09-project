using Microsoft.EntityFrameworkCore;
using StorageService.Api.Application.Interfaces;
using StorageService.Api.Application.Services;
using StorageService.Api.Infrastructure.Data;
using StorageService.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Database
var pgHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
var pgPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
var pgUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
var pgPass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres";
var pgDb = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "storagedb";

var conn = $"Host={pgHost};Port={pgPort};Username={pgUser};Password={pgPass};Database={pgDb}";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options
            .UseSqlite("Data Source=products-service-db.db")
            .UseSeeding((contextToSeed, _) => ApplicationDbContext.SeedData(contextToSeed))
            .UseAsyncSeeding((contextToSeed, _, _) =>
            {
                ApplicationDbContext.SeedData(contextToSeed);
                return Task.CompletedTask;
            });
    }
    else
        options.UseNpgsql(conn);
});


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (app.Environment.IsDevelopment())
        context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }