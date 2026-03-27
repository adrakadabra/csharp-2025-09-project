using System;

namespace ShiftService.Api.DTO
{
    /// <summary>
    /// Запрос на начало смены
    /// </summary>
    public class StartShiftRequest
    {
        public Guid EmployeeId { get; set; }
    }

    /// <summary>
    /// Запрос на завершение смены
    /// </summary>
    public class EndShiftRequest
    {
        public Guid EmployeeId { get; set; }
    }
}