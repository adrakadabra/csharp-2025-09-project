using System;

namespace ShiftService.Domain.Entities
{
    /// <summary>
    /// Связь сотрудника с пользователем Keycloak
    /// </summary>
    public class EmployeeUserMapping
    {
        public EmployeeUserMapping()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; set; }                    // ID записи

        public Guid EmployeeId { get; set; }            // ID сотрудника в ShiftService
        public virtual Employee Employee { get; set; }

        public string KeycloakUserId { get; set; }       // ID пользователя в Keycloak
        public string KeycloakUsername { get; set; }     // Имя пользователя в Keycloak

        public DateTime CreatedAt { get; set; }          // Когда создана связь
        public DateTime? LastLoginAt { get; set; }       // Последний вход (nullable)
    }
}