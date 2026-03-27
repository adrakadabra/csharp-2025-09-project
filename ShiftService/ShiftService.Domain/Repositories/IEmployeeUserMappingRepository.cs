using ShiftService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ShiftService.Domain.Repositories
{
    public interface IEmployeeUserMappingRepository
    {
        /// <summary>
        /// Получить связь по ID сотрудника
        /// </summary>
        Task<EmployeeUserMapping> GetByEmployeeIdAsync(Guid employeeId);

        /// <summary>
        /// Получить связь по ID пользователя Keycloak
        /// </summary>
        Task<EmployeeUserMapping> GetByKeycloakUserIdAsync(string keycloakUserId);

        /// <summary>
        /// Получить связь по имени пользователя Keycloak
        /// </summary>
        Task<EmployeeUserMapping> GetByKeycloakUsernameAsync(string username);

        /// <summary>
        /// Добавить связь
        /// </summary>
        Task AddAsync(EmployeeUserMapping mapping);

        /// <summary>
        /// Обновить связь (для LastLoginAt)
        /// </summary>
        Task UpdateAsync(EmployeeUserMapping mapping);
    }
}