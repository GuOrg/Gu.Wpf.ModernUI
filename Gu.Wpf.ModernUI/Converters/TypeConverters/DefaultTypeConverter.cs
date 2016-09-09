namespace Gu.Wpf.ModernUI.TypeConverters
{
    using System;
    using System.Globalization;

    internal class DefaultTypeConverter : ITypeConverter<object>
    {
        private readonly Type type;

        public DefaultTypeConverter(Type type)
        {
            this.type = type;
        }

        public bool IsValid(object value)
        {
            return this.type.IsInstanceOfType(value);
        }

        public bool CanConvertTo(object value, CultureInfo culture)
        {
            return IsValid(value);
        }

        public object ConvertTo(object value, CultureInfo culture)
        {
            return value;
        }
    }
}
