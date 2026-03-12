using StorageService.Api.Common.Enums;

namespace StorageService.Api.Application.DTOs
{
    public class ReserveItemDto
    {
        public string Article { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
