namespace ShiftService.Api.DTO
{
    /// <summary>
    /// Ответ после успешного входа по QR
    /// </summary>
    public class QrLoginResponse
    {
        /// <summary>
        /// Постоянный токен доступа
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Тип токена (обычно Bearer)
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// Время жизни токена в секундах
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// ID сотрудника
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        public string EmployeeName { get; set; }
    }
}