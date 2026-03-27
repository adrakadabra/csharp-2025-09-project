using ShiftService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShiftService.Domain.Repositories
{
    public interface IShiftRepository
    {
        Task<Shift> GetActiveShiftByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<Shift>> GetActiveShiftsAsync();
        Task<Shift> GetByIdAsync(Guid shiftId);
        Task<T> GetByIdProjectedAsync<T>(Guid shiftId);
        Task AddAsync(Shift shift);
        Task UpdateAsync(Shift shift);
        Task<IEnumerable<Shift>> GetShiftsForDayAsync(DateTime date);
        Task<int> GetActiveCountAsync();


    }
}