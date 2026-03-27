namespace ShiftService.Kiosk.Models
{
    // Ответ при авторизации
    public record AuthResponse(
        string access_token, 
        string token_type, 
        int expires_in
    );

    // Данные сотрудника
    public record EmployeeResponse(
        Guid employeeId,
        string fullName,
        bool accessAllowed
    );

    // Данные смены
    public record ShiftResponse(
        Guid shiftId,
        DateTime startTime,
        DateTime? endTime,
        bool isActive
    );

    public record HourlyStatistics(
        int hour, 
        int startedCount,
        int endedCount,
        int activeCount
    );

    public record ShiftStatisticsResponse(
        List<HourlyStatistics> hourlyData,
        int totalActiveShifts,
        int totalStartedToday,
        int totalEndedToday);
}
