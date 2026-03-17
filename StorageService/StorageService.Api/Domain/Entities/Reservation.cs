using StorageService.Api.Common.Enums;

namespace StorageService.Api.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }

        public Guid OrderNumber { get; set; }

        public List<ReservationItem> Items { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsComplete => Items.All(i => i.ReservationStatus == ReservationItemStatus.Complete);
        public bool IsReserved => Items.All(i => i.ReservationStatus == ReservationItemStatus.Reserved);
        public bool IsInProgressAssembly => Items.Any(i => i.ReservationStatus == ReservationItemStatus.InAssembly);
        public bool IsCompleteAssembly => Items.All(i => i.ReservationStatus == ReservationItemStatus.InAssembly);
        public bool IsCanceled => Items.All(i => i.ReservationStatus == ReservationItemStatus.Canceled);
    }
}
