namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Converts string values to upper case.
    /// </summary>
    public class ToUpperConverter : MarkupConverter<string, string>
    {
        protected override string Convert(string value, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            return value.ToUpperInvariant();
        }

        protected override string ConvertBack(string value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
