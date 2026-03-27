using Microsoft.AspNetCore.Mvc;
using ShiftService.Api.DTO;
using ShiftService.Application.Interfaces;
using ShiftService.Domain.Entities;
using ShiftService.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ShiftService.Api.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeUserMappingRepository _mappingRepository;

        public EmployeesController(
            IEmployeeService employeeService,
            IEmployeeUserMappingRepository mappingRepository)
        {
            _employeeService = employeeService;
            _mappingRepository = mappingRepository;
        }

        /// <summary>
        /// Создание нового сотрудника с привязкой к существующему пользователю Keycloak
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
        {
            try
            {
                // Создаем сотрудника с уже известным Keycloak ID
                var employee = await _employeeService.CreateEmployeeAsync(
                    request.FullName,
                    request.QrCode,
                    request.KeycloakUserId,
                    request.KeycloakUsername);

                // Получаем связь для ответа
                var mapping = await _mappingRepository.GetByEmployeeIdAsync(employee.Id);

                var response = new EmployeeResponse
                {
                    EmployeeId = employee.Id,
                    FullName = employee.FullName,
                    QrCode = employee.QrCode,
                    AccessAllowed = employee.AccessAllowed,
                    CreatedAt = employee.CreatedAt,
                    UpdatedAt = employee.UpdatedAt,
                    KeycloakUserId = mapping?.KeycloakUserId,
                    KeycloakUsername = mapping?.KeycloakUsername,
                    KeycloakLinkedAt = mapping?.CreatedAt
                };

                return CreatedAtAction(nameof(GetEmployee),
                    new { employeeId = employee.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Получение сотрудника по ID
        /// </summary>
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployee(Guid employeeId)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeAsync(employeeId);
                var mapping = await _mappingRepository.GetByEmployeeIdAsync(employeeId);

                var response = new EmployeeResponse
                {
                    EmployeeId = employee.Id,
                    FullName = employee.FullName,
                    QrCode = employee.QrCode,
                    AccessAllowed = employee.AccessAllowed,
                    CreatedAt = employee.CreatedAt,
                    UpdatedAt = employee.UpdatedAt,
                    KeycloakUserId = mapping?.KeycloakUserId,
                    KeycloakUsername = mapping?.KeycloakUsername,
                    KeycloakLinkedAt = mapping?.CreatedAt
                };

                return Ok(response);
            }
            catch (InvalidOperationException)
            {
                return NotFound($"Сотрудник с ID {employeeId} не найден");
            }
        }

        /// <summary>
        /// Получение сотрудника по Keycloak User ID
        /// </summary>
        [HttpGet("by-keycloak/{keycloakUserId}")]
        public async Task<IActionResult> GetEmployeeByKeycloakId(string keycloakUserId)
        {
            var employee = await _employeeService.GetEmployeeByKeycloakIdAsync(keycloakUserId);

            if (employee == null)
                return NotFound($"Сотрудник для пользователя Keycloak {keycloakUserId} не найден");

            var mapping = await _mappingRepository.GetByKeycloakUserIdAsync(keycloakUserId);

            var response = new EmployeeResponse
            {
                EmployeeId = employee.Id,
                FullName = employee.FullName,
                QrCode = employee.QrCode,
                AccessAllowed = employee.AccessAllowed,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt,
                KeycloakUserId = mapping?.KeycloakUserId,
                KeycloakUsername = mapping?.KeycloakUsername,
                KeycloakLinkedAt = mapping?.CreatedAt
            };

            return Ok(response);
        }

        /// <summary>
        /// Получение сотрудника по QR
        /// </summary>
        [HttpPost("by-qrcode")]
        public async Task<IActionResult> GetEmployeeByQrCode([FromBody] QrLoginRequest request)
        {

            var employee = await _employeeService.GetEmployeeByQrCode(request.QrCode);

            if (employee == null)
                return NotFound($"Сотрудник c qr кодом {request.QrCode} не найден");

            //var mapping = await _mappingRepository.GetByKeycloakUserIdAsync(request.QrCode);

            var response = new EmployeeResponse
            {
                EmployeeId = employee.Id,
                FullName = employee.FullName,
                QrCode = employee.QrCode,
                AccessAllowed = employee.AccessAllowed,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt,
              //  KeycloakUserId = mapping?.KeycloakUserId,
              //  KeycloakUsername = mapping?.KeycloakUsername,
              //  KeycloakLinkedAt = mapping?.CreatedAt
            };

            return Ok(response);
        }



        /// <summary>
        /// Обновление QR кода
        /// </summary>
        [HttpPatch("{employeeId}/qr")]
        public async Task<IActionResult> UpdateQr(Guid employeeId, [FromBody] string qrCode)
        {
            try
            {
                await _employeeService.UpdateEmployeeQrAsync(employeeId, qrCode);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Обновление имени сотрудника
        /// </summary>
        [HttpPatch("{employeeId}/name")]
        public async Task<IActionResult> UpdateName(Guid employeeId, [FromBody] string fullName)
        {
            try
            {
                await _employeeService.UpdateEmployeeNameAsync(employeeId, fullName);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Разрешить доступ
        /// </summary>
        [HttpPost("{employeeId}/allow-access")]
        public async Task<IActionResult> AllowAccess(Guid employeeId)
        {
            try
            {
                await _employeeService.AllowAccessAsync(employeeId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Запретить доступ
        /// </summary>
        [HttpPost("{employeeId}/deny-access")]
        public async Task<IActionResult> DenyAccess(Guid employeeId)
        {
            try
            {
                await _employeeService.DenyAccessAsync(employeeId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}