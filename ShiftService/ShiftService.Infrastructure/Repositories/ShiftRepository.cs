using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShiftService.Domain.Entities;
using ShiftService.Domain.Repositories;
using ShiftService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShiftService.Infrastructure.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly ShiftDbContext _context;
        private readonly IConfigurationProvider _mapperConfig; //маппер

        public ShiftRepository(ShiftDbContext context, IMapper mapper)
        {
            _context = context;
            _mapperConfig = mapper.ConfigurationProvider;
        }

        public async Task<Shift> GetActiveShiftByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.Shifts
                .Where(s => s.EmployeeId == employeeId && s.EndTime == null)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Shift>> GetActiveShiftsAsync()
        {
            return await _context.Shifts
                .Include(s => s.Employee)
                .Where(s => s.EndTime == null)
                .ToListAsync();
        }

        public async Task<Shift> GetByIdAsync(Guid shiftId)
        {
            return await _context.Shifts
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.Id == shiftId);
        }

        public async Task AddAsync(Shift shift)
        {
            await _context.Shifts.AddAsync(shift);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Shift shift)
        {
            _context.Shifts.Update(shift);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Shift>> GetShiftsForDayAsync(DateTime utcStart)
        {
            var utcEnd = utcStart.AddDays(1);

            return await _context.Shifts
                .Include(s => s.Employee)
                .Where(s =>
                    // 1. Началась сегодня
                    (s.StartTime >= utcStart && s.StartTime < utcEnd) ||
                    // 2. Закончилась сегодня
                    (s.EndTime != null && s.EndTime >= utcStart && s.EndTime < utcEnd) ||
                    // 3. Длится сквозь все текущие сутки
                    (s.StartTime < utcStart && (s.EndTime == null || s.EndTime >= utcEnd))
                )
                .ToListAsync();
        }

        //количество активных сотрудников 
        public async Task<int> GetActiveCountAsync()
        {
            // Возвращает количество всех открытых смен в системе
            return await _context.Shifts
                .CountAsync(s => s.EndTime == null);
        }

        // Универсальный метод для проекции в любой DTO
        public async Task<T> GetByIdProjectedAsync<T>(Guid shiftId)
        {
            return await _context.Shifts
                .Where(s => s.Id == shiftId)
                .ProjectTo<T>(_mapperConfig)
                .FirstOrDefaultAsync();
        }
    }
}