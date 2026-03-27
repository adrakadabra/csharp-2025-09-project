using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShiftService.Domain.Entities;
using ShiftService.Application.Interfaces;
using ShiftService.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShiftService.Domain.Repositories;
using TimeZoneConverter;


namespace ShiftService.Application.Services
{
    public class ShiftManagementService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ShiftManagementService(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
        {
            _shiftRepository = shiftRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<ShiftResponse> StartShiftAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new InvalidOperationException("Сотрудник не найден");

            if (!employee.AccessAllowed)
                throw new InvalidOperationException("Сотрудник не имеет доступа");

            // Проверяем, нет ли уже активной смены
            var activeShift = await _shiftRepository.GetActiveShiftByEmployeeIdAsync(employeeId);
            if (activeShift != null)
                throw new InvalidOperationException("У сотрудника уже есть активная смена");

            var shift = new Shift(employeeId);
            
            await _shiftRepository.AddAsync(shift);

            //Возвращаем DTO через проекцию
            return await _shiftRepository.GetByIdProjectedAsync<ShiftResponse>(shift.Id);
        }

        public async Task<ShiftResponse> EndShiftAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new InvalidOperationException("Сотрудник не найден");

            // ищем активную смену
            var activeShift = await _shiftRepository.GetActiveShiftByEmployeeIdAsync(employeeId);
            if (activeShift == null)
                throw new InvalidOperationException("У сотрудника нет активой смены");

            activeShift.End();
            
            await _shiftRepository.UpdateAsync(activeShift);

            //Возвращаем DTO через проекцию
            return await _shiftRepository.GetByIdProjectedAsync<ShiftResponse>(activeShift.Id);
        }

        public async Task<IEnumerable<Shift>> GetActiveShiftsAsync()
        {
            return await _shiftRepository.GetActiveShiftsAsync();
        }

        public async Task<Shift> GetActiveShiftByEmployeeAsync(Guid employeeId)
        {
            return await _shiftRepository.GetActiveShiftByEmployeeIdAsync(employeeId);
        }

        public async Task<ShiftStatisticsResponse> GetStatisticsForTodayAsync(string timeZoneId)
        {
            //Получаем объект часового пояса (TimeZoneConverter для кроссплатформенности)
            if (!TimeZoneConverter.TZConvert.TryGetTimeZoneInfo(timeZoneId, out var tz))
            {
                throw new ArgumentException($"Некорректный часовой пояс: {timeZoneId}");
            }

            //Вычисляем текущее время и начало текущих суток в этом поясе
            var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
            var localTodayStart = localNow.Date; // Ровно 00:00:00 локального дня

            //Определяем границы суток в UTC для запроса к БД
            var utcStart = TimeZoneInfo.ConvertTimeToUtc(localTodayStart, tz);
            var utcEnd = utcStart.AddDays(1);

            //Запрашиваем данные (захватываем смены, которые начались раньше, но еще длятся)
            var shifts = await _shiftRepository.GetShiftsForDayAsync(utcStart);

            var hourlyData = new List<HourlyStatistics>();

            for (int hour = 0; hour < 24; hour++)
            {
                // Локальные границы конкретного часа
                var localHourStart = localTodayStart.AddHours(hour);
                var localHourEnd = localHourStart.AddHours(1);

                // Переводим их в UTC для сравнения с метками в БД
                var utcHourStart = TimeZoneInfo.ConvertTimeToUtc(localHourStart, tz);
                var utcHourEnd = TimeZoneInfo.ConvertTimeToUtc(localHourEnd, tz);

                // Считаем статистику по UTC-меткам из базы
                // Начали в интервале часа [08:00:00, 09:00:00]
                var started = shifts.Count(s => s.StartTime >= utcHourStart && s.StartTime < utcHourEnd);
                // Закончили в интервале часа [08:00:00, 09:00:00)
                var ended = shifts.Count(s => s.EndTime.HasValue && s.EndTime.Value >= utcHourStart && s.EndTime.Value < utcHourEnd);

                // активные на момент конец часа utcHourEnd
                // Условие: смена началась до конца часа И (еще не закончилась ИЛИ закончилась после конца часа)
                var activeAtEndOfHour = shifts.Count(s =>
                    s.StartTime < utcHourEnd &&
                    (s.EndTime == null || s.EndTime > utcHourEnd));
                
                hourlyData.Add(new HourlyStatistics
                {
                    Hour = hour, // возвращаем локальный час (0..23)
                    StartedCount = started,
                    EndedCount = ended,
                    ActiveCount = activeAtEndOfHour
                });
            }

            return new ShiftStatisticsResponse
            {
                HourlyData = hourlyData,
                TotalActiveShifts = shifts.Count(s => s.EndTime == null),
                TotalStartedToday = shifts.Count(s => s.StartTime >= utcStart && s.StartTime < utcEnd),
                TotalEndedToday = shifts.Count(s => s.EndTime >= utcStart && s.EndTime < utcEnd)
            };
        }
        
        public async Task<int> GetCurrentActiveEmployeesCountAsync()
        {
            // Нам не важна дата начала, нам важен только факт отсутствия EndTime
            return await _shiftRepository.GetActiveCountAsync();
        }
    }
}