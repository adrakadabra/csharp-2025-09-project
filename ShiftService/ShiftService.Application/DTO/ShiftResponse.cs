using System;

namespace ShiftService.Application.DTO
{
    public class ShiftResponse
    {
        public Guid ShiftId { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}