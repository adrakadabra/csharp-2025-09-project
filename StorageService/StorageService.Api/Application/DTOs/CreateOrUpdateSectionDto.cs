namespace StorageService.Api.Application.DTOs
{
    public class CreateOrUpdateSectionDto
    {
        public string? Code { get; set; }   // Например, A1, B3
        public string? Description { get; set; }
    }
}
