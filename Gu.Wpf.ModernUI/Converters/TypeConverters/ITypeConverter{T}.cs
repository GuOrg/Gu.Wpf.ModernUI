namespace Gu.Wpf.ModernUI.TypeConverters
{
    using System.Globalization;

    /// <summary>
    /// A converter for converting to <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    public interface ITypeConverter<out T> : ITypeConverter
    {
        new T ConvertTo(object value, CultureInfo culture);
    }
}
