using ApexCharts;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShiftService.Kiosk;
using ShiftService.Kiosk.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Читаем BaseUrl из конфигурации
//var baseUrl = builder.Configuration["ShiftServiceApi:BaseUrl"] ?? "http://localhost:5000/";
var baseUrl = "http://localhost:5023/";
// Регистрируем именованный или обычный HttpClient
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(baseUrl)
});

// Регистрируем наш основной сервис
builder.Services.AddScoped<KioskApiService>();
builder.Services.AddApexCharts();

await builder.Build().RunAsync();