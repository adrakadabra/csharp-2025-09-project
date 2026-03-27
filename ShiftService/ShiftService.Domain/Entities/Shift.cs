using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftService.Domain.Entities
{
    /// <summary>
    /// Смена сотрудника
    /// </summary>
    public class Shift
    {
        public Shift()
        {
            Id = Guid.NewGuid();
            StartTime = DateTime.UtcNow;
        }
     
        public Shift(Guid employeeId) : this()
        {
            EmployeeId = employeeId;
        }

        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        // Навигационное свойство
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Активна ли смена (ещё не завершена)
        /// </summary>
        public bool IsActive => EndTime == null;

        /// <summary>
        /// Завершить смену текущим временем
        /// </summary>
        public void End()
        {
            if (EndTime != null)
                throw new InvalidOperationException("Смена уже завершена");
            EndTime = DateTime.UtcNow;
        }
    }
}
