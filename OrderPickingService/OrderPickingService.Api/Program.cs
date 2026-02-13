using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using OrderPickingService.Api.Services;
using OrderPickingService.Infrastructure.Database;
using OrderPickingService.Services;

namespace OrderPickingService.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите JWT токен в формате: Bearer {токен}"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => //TODO: вынести в настройки
            {
                options.Authority = "http://localhost:8080/realms/csharp-2025-09-project";
                options.Audience = "account";//"order-picking-service";
                options.RequireHttpsMetadata = false; // Для локальной разработки
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", 
                    NameClaimType = "preferred_username" 
                };
            });
        services
            .AddScoped<IClaimsTransformation, KeycloakRolesTransformer>()
            .AddAuthorization()
            .AddDatabase(builder.Configuration)
            .AddDomainServices()
            .AddValidatorsFromAssembly(typeof(Program).Assembly)
            ;
        
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHealthChecks("/health");
        app.UseHealthChecks("/order_picking_service", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains("order_picking_service")
        });
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}