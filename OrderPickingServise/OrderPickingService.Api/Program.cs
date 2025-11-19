using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using OrderPickingService.Api.Services;

namespace OrderPickingService.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
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

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        builder.Services.AddScoped<IClaimsTransformation, KeycloakRolesTransformer>();
        builder.Services.AddAuthorization();
        
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}