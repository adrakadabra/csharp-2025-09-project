namespace StorageService.Api.Application.DTOs
{
    public class ReserveProductsDto
    {
        public Guid OrderNumber { get; set; }
        public List<ReserveItemDto> Items { get; set; } = new();
    }
}
