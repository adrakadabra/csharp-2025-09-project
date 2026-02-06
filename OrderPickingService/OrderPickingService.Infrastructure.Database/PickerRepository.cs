using Microsoft.EntityFrameworkCore;
using OrderPickingService.Domain.Entities;
using OrderPickingService.Infrastructure.Database.Entities;
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Infrastructure.Database;

internal sealed class PickerRepository(DatabaseContext databaseContext) : IPickerRepository
{
    public async Task<List<Picker>> GetAllAsync()
    {
        return await databaseContext.Pickers
            .Select( x => Picker.Load(x.Id, x.FirstName, x.LastName))
            .ToListAsync();
    }

    public async Task<Picker?> GetByIdAsync(long id)
    {
        var picker = await databaseContext.Pickers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return picker == null ? null : Picker.Load(picker.Id, picker.FirstName, picker.LastName);
    }

    public async Task<Picker> CreateAsync(Picker picker)
    {
        var pickerEntity = PickerEntity.Create(
            picker.FirstName,
            picker.LastName);
        
        await databaseContext.Pickers.AddAsync(pickerEntity);
        await databaseContext.SaveChangesAsync();
        
        return Picker.Load(pickerEntity.Id, pickerEntity.FirstName, pickerEntity.LastName);
    }

    public async Task UpdateAsync(Picker picker)
    {
        var entity = await databaseContext.Pickers.FindAsync(picker.Id);
        
        if(entity == null)
            throw new KeyNotFoundException($"Picker with id {picker.Id} not found in database");
        
        if(entity.FirstName == picker.FirstName && 
           entity.LastName == picker.LastName)
            return;
        
        entity.FirstName = picker.FirstName;
        entity.LastName = picker.LastName;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = "system";
        
        await databaseContext.SaveChangesAsync();
    }
}