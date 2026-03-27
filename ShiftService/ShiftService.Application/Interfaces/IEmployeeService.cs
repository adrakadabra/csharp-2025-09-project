using ShiftService.Domain.Entities;

namespace ShiftService.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> CreateEmployeeAsync(string fullName, string qrCode, string keycloakUserId, string keycloakUsername, CancellationToken cancellationToken = default);
        Task<Employee> GetEmployeeAsync(Guid employeeId);
        Task<Employee> GetEmployeeByQrCode(string keycloakUserId);
        Task<Employee> GetEmployeeByKeycloakIdAsync(string keycloakUserId);
        Task UpdateEmployeeQrAsync(Guid employeeId, string qrCode);
        Task UpdateEmployeeNameAsync(Guid employeeId, string fullName);
        Task AllowAccessAsync(Guid employeeId);
        Task DenyAccessAsync(Guid employeeId);
        Task<bool> HasAccessAsync(Guid employeeId);
    }
}