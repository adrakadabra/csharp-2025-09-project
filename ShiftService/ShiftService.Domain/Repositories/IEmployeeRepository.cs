using ShiftService.Domain.Entities;

namespace ShiftService.Domain.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с сотрудниками
    /// Определяет контракт для доступа к данным
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Добавить нового сотрудника
        /// </summary>
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить сотрудника по ИД
        /// </summary>
        Task<Employee> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить сотрудника по qr коду
        /// </summary>
        Task<Employee?> GetByQrCodeAsync(string qrCode, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Проверить существование сотрудника по ИД
        /// </summary>
        Task<bool> ExistsAsync(Guid employeeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Проверить существование сотрудника по QR коду
        /// </summary>
        Task<bool> ExistsByQrCodeAsync(string qrCode, CancellationToken cancellationToken = default);
    }
}