using System.Text.Json;
using OrderPickingService.Infrastructure.Database.Entities.Order;
using OrderPickingService.Infrastructure.Database.Entities.Picker;

namespace OrderPickingService.Infrastructure.Database.Seed;

public static class JsonSeedDataLoader
{
    public static List<PickerEntity> LoadPickersFromJson()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Seed", "DataSeed", "pickers.json");
        return LoadFromFile<List<PickerEntity>>(filePath);
    }

    public static List<OrderEntity> LoadOrdersFromJson()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Seed", "DataSeed", "orders.json");
        return LoadFromFile<List<OrderEntity>>(filePath);
    }

    private static T LoadFromFile<T>(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Seed file not found: {filePath}");

        var json = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(json, options) 
               ?? throw new InvalidOperationException($"Failed to deserialize {filePath}");
    }
}