using System.ComponentModel.DataAnnotations;

namespace ShiftService.Api.DTO
{
    /// <summary>
    /// Запрос на создание сотрудника
    /// </summary>
    public class CreateEmployeeRequest
    {
        [Required(ErrorMessage = "Имя сотрудника обязательно")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 200 символов")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "QR код обязателен")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "QR код должен содержать от 1 до 100 символов")]
        public string QrCode { get; set; }
    }
}