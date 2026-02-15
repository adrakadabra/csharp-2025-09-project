namespace StorageService.Api.Application.DTOs
{
    public class SectionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;   // Например, A1, B3
        public string? Description { get; set; }
    }
}
