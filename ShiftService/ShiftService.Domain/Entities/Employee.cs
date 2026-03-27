using ShiftService.Domain.Common.ValueObjects;

//namespace ShiftService.Domain.Entities
//{
//    /// <summary>
//    /// Сущность сотрудника - ядро домена (DDD)
//    /// Содержит бизнес-логику и правила работы с сотрудником
//    /// </summary>
//    public class Employee
//    {
//        // Приватный конструктор для Entity Framework Core
//        // Нужен, чтобы EF мог создавать объекты при загрузке из БД
//        private Employee() { }

//        /// <summary>
//        /// Публичный конструктор для создания нового сотрудника
//        /// </summary>
//        /// <param name="fullName">Полное имя сотрудника</param>
//        /// <param name="qrCode">QR код сотрудника</param>
//        public Employee(string fullName, string qrCode)
//        {
//            // Генерируем новый уникальный идентификатор
//            EmployeeId = Guid.NewGuid();

//            // Валидация входных данных
//            if (string.IsNullOrWhiteSpace(fullName))
//                throw new ArgumentException("Имя сотрудника не может быть пустым");

//            if (string.IsNullOrWhiteSpace(qrCode))
//                throw new ArgumentException("QR код не может быть пустым");

//            FullName = fullName;
//            QrCode = qrCode;

//            // По умолчанию доступ запрещен
//            AccessAllowed = false;
//        }

//        // Свойства только для чтения извне (инкапсуляция)
//        // Значения можно изменить только через методы
//        public Guid EmployeeId { get; private set; }
//        public string FullName { get; private set; }
//        public string QrCode { get; private set; }
//        public bool AccessAllowed { get; private set; }

//        // Навигационное свойство для связи с пользователем Keycloak
//        public virtual EmployeeUserMapping UserMapping { get; set; }

//        /// <summary>
//        /// Обновление QR кода сотрудника
//        /// </summary>
//        /// <param name="qr">Новый QR код</param>
//        public void UpdateQr(string qr)
//        {
//            // Бизнес-правило: QR код не может быть пустым
//            if (string.IsNullOrWhiteSpace(qr))
//                throw new ArgumentException("QR код не может быть пустым");

//            QrCode = qr;
//        }

//        /// <summary>
//        /// Обновление имени сотрудника
//        /// </summary>
//        /// <param name="fullName">Новое имя</param>
//        public void UpdateName(string fullName)
//        {
//            // Бизнес-правило: имя не может быть пустым
//            if (string.IsNullOrWhiteSpace(fullName))
//                throw new ArgumentException("Имя сотрудника не может быть пустым");

//            FullName = fullName;
//        }

//        /// <summary>
//        /// Разрешить доступ сотруднику
//        /// </summary>
//        public void AllowAccess()
//        {
//            AccessAllowed = true;
//        }

//        /// <summary>
//        /// Запретить доступ сотруднику
//        /// </summary>
//        public void DenyAccess()
//        {
//            AccessAllowed = false;
//        }
//    }
//}


using System;

namespace ShiftService.Domain.Entities
{
    public class Employee
    {
        public Employee()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            AccessAllowed = false;
        }

        public Employee(string fullName, string qrCode) : this()
        {
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            QrCode = qrCode ?? throw new ArgumentNullException(nameof(qrCode));
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string QrCode { get; set; }
        public bool AccessAllowed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual EmployeeUserMapping UserMapping { get; set; }

        public void UpdateQr(string qrCode)
        {
            if (string.IsNullOrWhiteSpace(qrCode))
                throw new ArgumentException("QR код не может быть пустым");

            QrCode = qrCode;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Имя не может быть пустым");

            FullName = fullName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AllowAccess()
        {
            AccessAllowed = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DenyAccess()
        {
            AccessAllowed = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}