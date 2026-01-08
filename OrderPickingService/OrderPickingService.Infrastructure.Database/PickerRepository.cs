
using Microsoft.EntityFrameworkCore;
using OrderPickingService.Domain.Entities;
using OrderPickingService.Infrastructure.Database.Abstractions;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Infrastructure.Database;

internal sealed class PickerRepository(IDataBaseContext databaseContext) : IPickerRepository
{
    public Task<List<Picker>> GetPickers()
    {
        return databaseContext.Pickers
            .Select( x => Picker.Create(x.Id, x.FirstName, x.LastName))
            .ToListAsync();
    }
}