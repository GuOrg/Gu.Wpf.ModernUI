namespace Gu.Wpf.ModernUI.TypeConverters
{
    using System.Globalization;

    using Gu.Wpf.ModernUI.Converters.TypeConverters;

    public class CastingConverter<T> : ITypeConverter<T>
    {
        private readonly ITypeConverter<object> inner;

        public CastingConverter(ITypeConverter<object> inner )
        {
            this.inner = inner;
        }

        public bool IsValid(object value)
        {
            return this.inner.IsValid(value);
        }

        public bool CanConvertTo(object value, CultureInfo culture)
        {
            return this.inner.CanConvertTo(value, culture);
        }

        public T ConvertTo(object value, CultureInfo culture)
        {
            return (T)this.inner.ConvertTo(value, culture);
        }

        object ITypeConverter.ConvertTo(object value, CultureInfo culture)
        {
            return ConvertTo(value, culture);
        }
    }
}
