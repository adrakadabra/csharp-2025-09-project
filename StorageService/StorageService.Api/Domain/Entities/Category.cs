namespace StorageService.Api.Domain.Entities
{
    // Категория товара
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public bool IsDeleted { get; set; } = false;
        public ICollection<Product> Products { get; set; } = [];
    }
}
