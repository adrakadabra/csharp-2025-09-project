using ShiftService.Domain.Entities;
using ShiftService.Domain.Interfaces;
using ShiftService.Domain.Repositories;
using System;
using System.Threading.Tasks;
using ShiftService.Application.Interfaces;

namespace ShiftService.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeUserMappingRepository _mappingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IEmployeeUserMappingRepository mappingRepository,
            IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _mappingRepository = mappingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Employee> CreateEmployeeAsync(
            string fullName,
            string qrCode,
            string keycloakUserId,
            string keycloakUsername,
            System.Threading.CancellationToken cancellationToken = default)
        {
            //Проверяем, не существует ли уже сотрудник с таким QR кодом
            var existingByQr = await _employeeRepository.GetByQrCodeAsync(qrCode);
            if (existingByQr != null)
                throw new InvalidOperationException($"Сотрудник с QR кодом {qrCode} уже существует");

            //Проверяем, не привязан ли уже этот Keycloak пользователь к другому сотруднику
            var existingMapping = await _mappingRepository.GetByKeycloakUserIdAsync(keycloakUserId);
            if (existingMapping != null)
                throw new InvalidOperationException($"Пользователь Keycloak {keycloakUserId} уже привязан к другому сотруднику");

            //Создаем сотрудника
            var employee = new Employee(fullName, qrCode);
            // add without committing so we can add mapping in one transaction
            await _employeeRepository.AddAsync(employee, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken); // Явное сохранение

            // 4. Создаем связь с Keycloak
            var mapping = new EmployeeUserMapping
            {
                EmployeeId = employee.Id,
                KeycloakUserId = keycloakUserId,
                KeycloakUsername = keycloakUsername,
                CreatedAt = DateTime.UtcNow
            };
            await _mappingRepository.AddAsync(mapping);

            // 5. Загружаем сотрудника вместе со связью
            return await _employeeRepository.GetByIdAsync(employee.Id);
        }

        public async Task<Employee> GetEmployeeAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new InvalidOperationException($"Сотрудник с ID {employeeId} не найден");
            return employee;
        }

        public async Task<Employee> GetEmployeeByQrCode(string qrСode)
        {
            return  await _employeeRepository.GetByQrCodeAsync(qrСode);
        }

        public async Task<Employee> GetEmployeeByKeycloakIdAsync(string keycloakUserId)
        {
            var mapping = await _mappingRepository.GetByKeycloakUserIdAsync(keycloakUserId);
            if (mapping == null)
                return null;

            return await _employeeRepository.GetByIdAsync(mapping.EmployeeId);
        }

        public async Task UpdateEmployeeQrAsync(Guid employeeId, string qrCode)
        {
            var employee = await GetEmployeeAsync(employeeId);

            var existing = await _employeeRepository.GetByQrCodeAsync(qrCode);
            if (existing != null && existing.Id != employeeId)
                throw new InvalidOperationException($"QR код {qrCode} уже используется");

            employee.UpdateQr(qrCode);
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task UpdateEmployeeNameAsync(Guid employeeId, string fullName)
        {
            var employee = await GetEmployeeAsync(employeeId);
            employee.UpdateName(fullName);
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task AllowAccessAsync(Guid employeeId)
        {
            var employee = await GetEmployeeAsync(employeeId);
            employee.AllowAccess();
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task DenyAccessAsync(Guid employeeId)
        {
            var employee = await GetEmployeeAsync(employeeId);
            employee.DenyAccess();
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<bool> HasAccessAsync(Guid employeeId)
        {
            var employee = await GetEmployeeAsync(employeeId);
            return employee.AccessAllowed;
        }
    }
}