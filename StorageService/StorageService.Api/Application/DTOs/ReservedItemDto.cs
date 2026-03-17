using StorageService.Api.Common.Enums;

namespace StorageService.Api.Application.DTOs
{
    public class ReservedItemDto
    {
        public string Article { get; set; } = null!;
        public int Quantity { get; set; }
        public ReservationItemStatus ReservationItemStatus { get; set; }
    }
}
