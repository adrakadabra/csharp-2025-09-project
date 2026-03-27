using Microsoft.EntityFrameworkCore;
using ShiftService.Domain.Entities;
using ShiftService.Domain.Repositories;
using ShiftService.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace ShiftService.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ShiftDbContext _context;

        public EmployeeRepository(ShiftDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByIdAsync(Guid employeeId, System.Threading.CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.UserMapping)
                .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);
        }

        public async Task<Employee> GetByQrCodeAsync(string qrCode, System.Threading.CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(qrCode))
                return null;

            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.UserMapping)
                .FirstOrDefaultAsync(e => e.QrCode == qrCode, cancellationToken);
        }

        public async Task AddAsync(Employee employee, System.Threading.CancellationToken cancellationToken = default)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));

            // Устанавливаем временные метки
            employee.CreatedAt = DateTime.UtcNow;
            employee.UpdatedAt = DateTime.UtcNow;

            await _context.Employees.AddAsync(employee, cancellationToken);
        }
        
        // Синхронные методы - без async, с Task.CompletedTask
        public Task UpdateAsync(Employee employee, System.Threading.CancellationToken cancellationToken = default)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));

            employee.UpdatedAt = DateTime.UtcNow;
            _context.Employees.Update(employee);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid employeeId, System.Threading.CancellationToken cancellationToken = default)
        {
            return await _context.Employees.AsNoTracking().AnyAsync(e => e.Id == employeeId, cancellationToken);
        }
        public async Task<bool> ExistsByQrCodeAsync(string qrCode, System.Threading.CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(qrCode))
                return false;

            return await _context.Employees.AsNoTracking().AnyAsync(e => e.QrCode == qrCode, cancellationToken);
        }
    }
}