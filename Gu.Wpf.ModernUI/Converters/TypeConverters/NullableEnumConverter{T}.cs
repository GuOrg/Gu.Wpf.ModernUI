namespace Gu.Wpf.ModernUI.TypeConverters
{
    using System;
    using System.Globalization;
    using System.Linq;

    internal class NullableEnumConverter<T> : ITypeConverter<T?>
        where T : struct, IComparable, IFormattable
    {
        internal static readonly NullableEnumConverter<T> Default = new NullableEnumConverter<T>();

        private static readonly Type[] ValidTypes =
        {
            typeof(T),
        };

        public bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (ValidTypes.Contains(value.GetType()))
            {
                return true;
            }

            return false;
        }

        public bool CanConvertTo(object value, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }

            if (ValidTypes.Contains(value.GetType()))
            {
                return true;
            }

            if (value is string s)
            {
                T temp;
                return Enum.TryParse(s, true, out temp);
            }

            return false;
        }

        public T? ConvertTo(object value, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (ValidTypes.Contains(value.GetType()))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            if (value is string s)
            {
                return (T)Enum.Parse(typeof(T), s);
            }

            throw new ArgumentException(nameof(value));
        }

        object ITypeConverter.ConvertTo(object value, CultureInfo culture)
        {
            return this.ConvertTo(value, culture);
        }
    }
}