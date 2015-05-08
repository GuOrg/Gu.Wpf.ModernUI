namespace Gu.Wpf.ModernUI.Converters.TypeConverters
{
    using System.Globalization;

    using ModernUI.TypeConverters;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITypeConverter<out T> : ITypeConverter
    {
        T ConvertTo(object value, CultureInfo culture);
    }
}
