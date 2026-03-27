namespace ShiftService.Api.DTO
{
    /// <summary>
    /// Стандартный ответ с ошибкой
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error { get; set; }
    }
}