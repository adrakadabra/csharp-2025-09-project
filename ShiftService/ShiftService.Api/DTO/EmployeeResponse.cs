using System;

namespace ShiftService.Api.DTO
{
    /// <summary>
    /// Ответ с данными сотрудника
    /// </summary>
    public class EmployeeResponse
    {
        /// <summary>
        /// ID сотрудника
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Полное имя сотрудника
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// QR код сотрудника
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// Разрешен ли доступ
        /// </summary>
        public bool AccessAllowed { get; set; }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Информация о привязке к Keycloak
        public string KeycloakUserId { get; set; }
        public string KeycloakUsername { get; set; }
        public DateTime? KeycloakLinkedAt { get; set; }

    }
}