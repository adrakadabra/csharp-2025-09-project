using ShiftService.Domain.Entities;
using ShiftService.Application.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShiftService.Application.Interfaces
{
    public interface IShiftService
    {
        Task<ShiftResponse> StartShiftAsync(Guid employeeId);
        Task<ShiftResponse> EndShiftAsync(Guid employeeId);
        Task<IEnumerable<Shift>> GetActiveShiftsAsync();
        Task<ShiftStatisticsResponse> GetStatisticsForTodayAsync(string timeZoneId = "UTC");
        Task<Shift> GetActiveShiftByEmployeeAsync(Guid employeeId);
        Task<int> GetCurrentActiveEmployeesCountAsync();
    }
}