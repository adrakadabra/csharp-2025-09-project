using Microsoft.EntityFrameworkCore;
using StorageService.Api.Application.DTOs;
using StorageService.Api.Application.Interfaces;
using StorageService.Api.Application.Mappers;
using StorageService.Api.Common.Enums;
using StorageService.Api.Infrastructure.Data;
using System.Collections.Concurrent;

namespace StorageService.Api.Application.Services
{
    public class ReservationItemsService : IReservationItemsService
    {
        private readonly ApplicationDbContext _context;

        private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> _locks = new();

        public ReservationItemsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task ChangeReservationItemStatusAsync(Guid orderNumber, string article, ReservationItemStatus status)
        {
            var semaphore = _locks.GetOrAdd(orderNumber, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();

            try
            {
                var reservationItem = await _context.ReservationItems
                    .Include(r => r.Reservation)
                    .Include(r => r.Product)
                    .FirstOrDefaultAsync(r => r.Reservation.OrderNumber == orderNumber && r.Product.Article == article);

                if (reservationItem == null)
                {
                    throw new InvalidOperationException("No reservation product for change status");
                }

                reservationItem.ReservationStatus = status;

                await _context.SaveChangesAsync();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<List<ReservedItemDto>> GetReservationItemsForOrder(Guid orderNumber)
        {
            return await _context.ReservationItems
                .Include(r => r.Reservation)
                .Include(r => r.Product)
                .Where(r => r.Reservation.OrderNumber == orderNumber).Select(r => r.ToDto()).ToListAsync();
        }
    }
}
