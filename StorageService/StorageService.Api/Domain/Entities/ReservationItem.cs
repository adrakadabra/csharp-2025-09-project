using StorageService.Api.Common.Enums;

namespace StorageService.Api.Domain.Entities
{
    public class ReservationItem
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public Guid ReservationId { get; set; }

        public Reservation Reservation { get; set; } = null!;

        public ReservationItemStatus ReservationStatus { get;set; } = ReservationItemStatus.Reserved;

        public void ChangeReservationItemStatus(ReservationItemStatus status)
        {
            ReservationStatus = status;
        }
    }
}
