using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftService.Application.Interfaces;      // для IKeycloakService
using ShiftService.Domain.Interfaces;
using ShiftService.Domain.Repositories;
using ShiftService.Infrastructure.Persistence;  // для ShiftDbContext
using ShiftService.Infrastructure.Repositories; // для EmployeeRepository
using ShiftService.Infrastructure.Services;     // для KeycloakService

namespace ShiftService.Infrastructure
{
    /// <summary>
    /// Класс для регистрации всех зависимостей инфраструктурного слоя
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Этот метод регистрирует ВСЕ сервисы инфраструктурного слоя
        /// Вызывается из Program.cs одной строкой
        /// </summary>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,    // методы расширения для IServiceCollection
            IConfiguration configuration)        // конфигурация для строк подключения
        {

            // Database
            var pgHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
            var pgPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5433";
            var pgUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
            var pgPass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres";
            var pgDb = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "shiftdb";

            var connectionString = $"Host={pgHost};Port={pgPort};Username={pgUser};Password={pgPass};Database={pgDb}";
            Console.WriteLine($"Строка подключения: {connectionString}");

            //РЕГИСТРАЦИЯ БАЗЫ ДАННЫХ
            //var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ShiftDbContext>(options =>
                options.UseNpgsql(connectionString));

            // ===== 2. РЕГИСТРАЦИЯ РЕПОЗИТОРИЕВ =====
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeUserMappingRepository, EmployeeUserMappingRepository>();
            services.AddScoped<IShiftRepository, ShiftRepository>();

            // ===== 2. РЕГИСТРАЦИЯ ИНТЕРФЕЙСОВ =====
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ===== 3. РЕГИСТРАЦИЯ ВНЕШНИХ СЕРВИСОВ =====
            services.AddHttpClient<IKeycloakService, KeycloakService>(client =>
            {
                var baseUrl = configuration["Keycloak:Authority"];
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    client.BaseAddress = new Uri(baseUrl);
                }
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            // Возвращаем services для возможности цепочки вызовов
            return services;
        }
    }
}