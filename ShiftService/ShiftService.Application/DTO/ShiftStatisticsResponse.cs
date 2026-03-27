using System.Collections.Generic;

namespace ShiftService.Application.DTO
{
    public class HourlyStatistics
    {
        public int Hour { get; set; }
        public int StartedCount { get; set; }
        public int EndedCount { get; set; }
        public int ActiveCount { get; set; }
    }

    public class ShiftStatisticsResponse
    {
        public List<HourlyStatistics> HourlyData { get; set; }
        public int TotalActiveShifts { get; set; }
        public int TotalStartedToday { get; set; }
        public int TotalEndedToday { get; set; }
    }
}