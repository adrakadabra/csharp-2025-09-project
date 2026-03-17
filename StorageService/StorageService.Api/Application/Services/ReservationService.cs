using Microsoft.EntityFrameworkCore;
using StorageService.Api.Application.DTOs;
using StorageService.Api.Application.Interfaces;
using StorageService.Api.Application.Mappers;
using StorageService.Api.Common.Enums;
using StorageService.Api.Domain.Entities;
using StorageService.Api.Infrastructure.Data;
using StorageService.Api.Infrastructure.Interfaces;
using System.Collections.Concurrent;

namespace StorageService.Api.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;

        private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> _locks = new();

        public ReservationService(
            IProductRepository productRepository,
            ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<ReservedOrderDto> ReserveProductsAsync(ReserveProductsDto dto)
        {
            var semaphore = _locks.GetOrAdd(dto.OrderNumber, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var reservation = new Reservation
                {
                    OrderNumber = dto.OrderNumber
                };

                foreach (var item in dto.Items)
                {
                    var product = await _productRepository.GetByArticleAsync(item.Article);

                    if (product == null)
                        throw new Exception($"Product {item.Article} not found");

                    product.Reserve(item.Quantity);

                    reservation.Items.Add(new ReservationItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity
                    });
                }

                var res = await _context.Reservations.AddAsync(reservation);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return res.Entity.ToDto();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task CancelReservationAsync(Guid orderNumber)
        {
            var semaphore = _locks.GetOrAdd(orderNumber, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var reservation = await _context.Reservations
                    .Include(r => r.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(r => r.OrderNumber == orderNumber);

                if (reservation == null)
                    throw new InvalidOperationException("Reservation not found");

                if (reservation.IsCanceled || reservation.IsComplete)
                {
                    throw new InvalidOperationException("Reservation is already in terminate status");
                }

                foreach (var item in reservation.Items)
                {
                    item.Product.Release(item.Quantity);
                    item.ReservationStatus = ReservationItemStatus.Canceled;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task CompleteReservationAsync(Guid orderNumber)
        {
            var semaphore = _locks.GetOrAdd(orderNumber, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var reservation = await _context.Reservations
                    .Include(r => r.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(r => r.OrderNumber == orderNumber);

                if (reservation == null)
                    throw new Exception("Reservation not found");

                if (reservation.IsCanceled || reservation.IsComplete)
                {
                    throw new InvalidOperationException("Reservation is already in terminate status");
                }

                foreach (var item in reservation.Items)
                {
                    item.Product.Complete(item.Quantity);
                    item.ReservationStatus = ReservationItemStatus.Complete;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<ReservedOrderDto> GetReservationAsync(Guid orderNumber)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(r => r.OrderNumber == orderNumber);

            if (reservation == null)
                throw new Exception("Reservation not found");

            return reservation.ToDto();
        }

        public async Task<List<ReservedOrderDto>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Items)
                .ThenInclude(i => i.Product)
                .Select(r => r.ToDto())
                .ToListAsync();
        }
    }
}
