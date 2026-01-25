namespace StorageService.Api.Application.DTOs
{
    public class ManufacturerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
    }
}
