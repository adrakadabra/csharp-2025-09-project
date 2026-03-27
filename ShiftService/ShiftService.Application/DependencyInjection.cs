using Microsoft.Extensions.DependencyInjection;
using ShiftService.Application.Interfaces;
using ShiftService.Application.Services;
using System.Reflection;


namespace ShiftService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Регистрируем AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Регистрируем сервисы приложения
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IShiftService, ShiftManagementService>();
            return services;
        }
    }
}