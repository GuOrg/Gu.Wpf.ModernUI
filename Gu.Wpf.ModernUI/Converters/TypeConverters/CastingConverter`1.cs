namespace Gu.Wpf.ModernUI.TypeConverters
{
    using System.Globalization;

    public class CastingConverter<T> : ITypeConverter<T>
    {
        private readonly ITypeConverter<object> inner;

        public CastingConverter(ITypeConverter<object> inner)
        {
            this.inner = inner;
        }

        /// <inheritdoc/>
        public bool IsValid(object value)
        {
            return this.inner.IsValid(value);
        }

        /// <inheritdoc/>
        public bool CanConvertTo(object value, CultureInfo culture)
        {
            return this.inner.CanConvertTo(value, culture);
        }

        /// <inheritdoc/>
        public T ConvertTo(object value, CultureInfo culture)
        {
            return (T)this.inner.ConvertTo(value, culture);
        }

        /// <inheritdoc/>
        object ITypeConverter.ConvertTo(object value, CultureInfo culture)
        {
            return ConvertTo(value, culture);
        }
    }
}
