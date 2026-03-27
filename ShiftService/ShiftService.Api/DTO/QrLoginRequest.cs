using System.ComponentModel.DataAnnotations;

namespace ShiftService.Api.DTO
{
    /// <summary>
    /// Запрос на вход по QR-коду
    /// </summary>
    public class QrLoginRequest
    {
        [Required(ErrorMessage = "QR код обязателен")]
        public string QrCode { get; set; }
    }
}