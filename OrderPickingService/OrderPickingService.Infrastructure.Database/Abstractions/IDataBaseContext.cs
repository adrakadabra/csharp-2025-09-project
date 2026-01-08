using Microsoft.EntityFrameworkCore;
using OrderPickingService.Infrastructure.Database.Dtos;

namespace OrderPickingService.Infrastructure.Database.Abstractions;

internal interface IDataBaseContext
{ 
    DbSet<Picker> Pickers { get; }
}