using Microsoft.EntityFrameworkCore;
using ShiftService.Domain.Entities;
using ShiftService.Domain.Repositories;
using ShiftService.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace ShiftService.Infrastructure.Repositories
{
    public class EmployeeUserMappingRepository : IEmployeeUserMappingRepository
    {
        private readonly ShiftDbContext _context;

        public EmployeeUserMappingRepository(ShiftDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeUserMapping> GetByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.EmployeeUserMappings
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.EmployeeId == employeeId);
        }

        public async Task<EmployeeUserMapping> GetByKeycloakUserIdAsync(string keycloakUserId)
        {
            if (string.IsNullOrWhiteSpace(keycloakUserId))
                return null;

            return await _context.EmployeeUserMappings
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.KeycloakUserId == keycloakUserId);
        }

        public async Task<EmployeeUserMapping> GetByKeycloakUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return await _context.EmployeeUserMappings
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.KeycloakUsername == username);
        }

        public async Task AddAsync(EmployeeUserMapping mapping)
        {
            if (mapping == null) throw new ArgumentNullException(nameof(mapping));

            // Id и CreatedAt устанавливаются в конструкторе
            await _context.EmployeeUserMappings.AddAsync(mapping);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmployeeUserMapping mapping)
        {
            _context.EmployeeUserMappings.Update(mapping);
            await _context.SaveChangesAsync();
        }
    }
}