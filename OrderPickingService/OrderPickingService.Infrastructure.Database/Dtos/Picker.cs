namespace OrderPickingService.Infrastructure.Database.Dtos;

internal class Picker : BaseEntity
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    private Picker() {}
}