using AuthService.Api.Models;
using AuthService.Api.Models.Keycloak;
using AuthService.Api.Services;

namespace AuthService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // 1. Регистрируем настройки Keycloak
        builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection("Keycloak"));
        
        // 2. Добавляем HttpClient для вызовов Keycloak

        builder.Services.AddHttpClient("Keycloak", client =>
        {
            client.BaseAddress = new Uri("http://localhost:8080/");
        });
        
        // 3. Регистрируем наш KeycloakService
        builder.Services.AddScoped<IKeycloakService, KeycloakService>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}