namespace ShiftService.Domain.Common.ValueObjects
{
    /// <summary>
    /// Value Object для QR кода
    /// </summary>
    public class QrCode
    {
        public string Value { get; }

        public QrCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("QR код не может быть пустым");

            if (value.Length > 100)
                throw new ArgumentException("QR код не может быть длиннее 100 символов");

            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is QrCode other && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}