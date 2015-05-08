namespace Gu.Wpf.ModernUI.TypeConverters
{
    using System;
    using System.Globalization;

    using Gu.Wpf.ModernUI.Converters.TypeConverters;

    internal class DefaultTypeConverter : ITypeConverter<object>
    {
        private readonly Type _type;

        public DefaultTypeConverter(Type type)
        {
            this._type = type;
        }

        public bool IsValid(object value)
        {
            return this._type.IsInstanceOfType(value);
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
