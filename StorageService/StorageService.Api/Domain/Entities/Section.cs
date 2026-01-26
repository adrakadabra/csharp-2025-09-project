namespace StorageService.Api.Domain.Entities
{
    // Секция на складе
    public class Section
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = null!;   // Например, A1, B3
        public string? Description { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
