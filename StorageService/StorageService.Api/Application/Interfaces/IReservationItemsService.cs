using StorageService.Api.Application.DTOs;
using StorageService.Api.Common.Enums;

namespace StorageService.Api.Application.Interfaces
{
    public interface IReservationItemsService
    {
        public Task ChangeReservationItemStatusAsync(Guid orderNumber, string article, ReservationItemStatus status);

        public Task<List<ReservedItemDto>> GetReservationItemsForOrder(Guid orderNumber);
    }
}
