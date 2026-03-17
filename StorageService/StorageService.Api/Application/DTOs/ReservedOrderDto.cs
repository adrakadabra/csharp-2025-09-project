namespace StorageService.Api.Application.DTOs
{
    public class ReservedOrderDto
    {
        public Guid OrderNumber { get; set; }
        public List<ReservedItemDto> Items { get; set; } = new();
    }
}
