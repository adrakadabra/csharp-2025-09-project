namespace StorageService.Api.Domain.Entities
{
    // Производитель
    public class Manufacturer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }

        public bool IsDeleted { get; set; } = false;
        public ICollection<Product> Products { get; set; } = [];
    }
}
