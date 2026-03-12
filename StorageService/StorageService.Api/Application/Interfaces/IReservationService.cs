using StorageService.Api.Application.DTOs;

namespace StorageService.Api.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservedOrderDto> ReserveProductsAsync(ReserveProductsDto dto);

        Task CancelReservationAsync(Guid orderNumber);

        Task CompleteReservationAsync(Guid orderNumber);

        Task<ReservedOrderDto> GetReservationAsync(Guid orderNumber);
        Task<List<ReservedOrderDto>> GetAllReservationsAsync();
    }
}
