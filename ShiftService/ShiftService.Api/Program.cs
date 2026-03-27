using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShiftService.Application;  // для AddApplication
using ShiftService.Infrastructure; // для AddInfrastructure
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


// НАСТРОЙКА JWT АУТЕНТИФИКАЦИИ
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Адрес Keycloak
        options.Authority = "http://localhost:8080/realms/master";
        options.Audience = "account";
        options.RequireHttpsMetadata = false; // Для разработки

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };

        // Логирование для отладки
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"Token validated: {context.Principal?.Identity?.Name}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"Challenge: {context.Error}, {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Добавляем сервисы в контейнер зависимостей
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    //options.AddPolicy("DefaultPolicy", policy =>
    //{
    //    policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost") // Разрешает любой порт на localhost
     //         .AllowAnyMethod()
    //          .AllowAnyHeader();
    //});
    options.AddPolicy("default", policy =>
    {
        policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials();
    });

});


// НАСТРОЙКА SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Shift Service API",
        Version = "v1",
        Description = "Микросервис для управления сотрудниками"
    });
    // Включаем XML комментарии
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    //Настройка JWT для Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите токен в формате: Bearer {ваш_токен}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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


// Диагностика
Console.WriteLine($"Текущая директория: {Directory.GetCurrentDirectory()}");
Console.WriteLine($"Файл appsettings.json существует: {File.Exists("appsettings.json")}");


// ===== РЕГИСТРАЦИЯ СЛОЕВ ЧЕРЕЗ DEPENDENCY INJECTION =====
builder.Services.AddInfrastructure(builder.Configuration);  // Регистрирует: DbContext, репозитории, KeycloakService
builder.Services.AddApplication();                           // Регистрирует: EmployeeService

var app = builder.Build();

//Статика и роутинг
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shift Service API V1");
    });
}
//ВКЛЮЧАЕМ CORS (обязательно до авторизации и контроллеров!)
app.UseCors("default");

//Маршрутизация и безопасность
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Применение миграций
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ShiftService.Infrastructure.Persistence.ShiftDbContext>();

        Console.WriteLine("Проверка подключения к базе данных...");

        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("Подключение к БД успешно!");

            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            var pendingMigrationList = pendingMigrations.ToList();

            if (pendingMigrationList.Any())
            {
                Console.WriteLine($"Найдено {pendingMigrationList.Count} ожидающих миграций:");
                foreach (var migration in pendingMigrationList)
                {
                    Console.WriteLine($"  - {migration}");
                }

                Console.WriteLine("Применение миграций...");
                await dbContext.Database.MigrateAsync();
                Console.WriteLine("Миграции успешно применены!");
            }
            else
            {
                Console.WriteLine("Все миграции уже применены.");
            }

            // Проверяем наличие таблицы Employees
            try
            {
                var employeeCount = await dbContext.Employees.CountAsync();
                Console.WriteLine($"Таблица Employees содержит {employeeCount} записей");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Таблица Employees еще не создана или недоступна: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("НЕ УДАЛОСЬ подключиться к БД!");
            Console.WriteLine("Проверьте строку подключения и запущен ли PostgreSQL");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ОШИБКА при работе с БД: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
    }
}

app.Run();