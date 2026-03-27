using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftService.Api.DTO;
using ShiftService.Application.DTO;
using ShiftService.Application.Interfaces;
using ShiftService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShiftService.Api.Controllers
{
    [ApiController]
    [Route("api/shifts")]
    //[Authorize]
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        private readonly IEmployeeRepository _employeeRepository;

        public ShiftsController(IShiftService shiftService, IEmployeeRepository employeeRepository)
        {
            _shiftService = shiftService;
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Начать смену сотрудника
        /// </summary>
        [HttpPost("start")]
        public async Task<IActionResult> StartShift([FromBody] StartShiftRequest request)
        {
            try
            {
                var response = await _shiftService.StartShiftAsync(request.EmployeeId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Завершить смену сотрудника
        /// </summary>
        [HttpPost("end")]
        public async Task<IActionResult> EndShift([FromBody] EndShiftRequest request)
        {
            try
            {
                var response = await _shiftService.EndShiftAsync(request.EmployeeId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Общее количество всех работающих сотрудников (включая незакрытые прошлые смены)
        /// </summary>
        [HttpGet("active/now")]
        public async Task<ActionResult<int>> GetCurrentActiveCount()
        {
            var count = await _shiftService.GetCurrentActiveEmployeesCountAsync();
            return Ok(count);
        }

        /// <summary>
        /// Получить статистику для диаграммы за текущие сутки
        /// </summary>
        [HttpGet("statistics/today")]
        public async Task<ActionResult<ShiftStatisticsResponse>> GetTodayStatistics([FromQuery] string timeZoneId = "UTC")
        {
            try
            {
                var stats = await _shiftService.GetStatisticsForTodayAsync(timeZoneId);
                return Ok(stats);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            } 
        }

        [HttpGet("current/{employeeId}")]
        public async Task<IActionResult> GetCurrentShift(Guid employeeId)
        {
            // Вызываем ваш существующий метод из ShiftService
            var activeShift = await _shiftService.GetActiveShiftByEmployeeAsync(employeeId);

            if (activeShift == null)
                return NotFound(); // Смены нет — это нормальная ситуация

            // Маппим в Response (DTO), который ожидает Кiosk
            var response = new ShiftResponse
            {
                ShiftId = activeShift.Id,
                EmployeeId = activeShift.EmployeeId,
                StartTime = activeShift.StartTime,
                EndTime = activeShift.StartTime,
                IsActive  = activeShift.IsActive
            };

            return Ok(response);
        }

    }
}