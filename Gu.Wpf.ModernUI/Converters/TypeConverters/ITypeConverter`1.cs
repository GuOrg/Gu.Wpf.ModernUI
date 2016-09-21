namespace Gu.Wpf.ModernUI.TypeConverters
{
    using System.Globalization;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITypeConverter<out T> : ITypeConverter
    {
        new T ConvertTo(object value, CultureInfo culture);
    }
}
