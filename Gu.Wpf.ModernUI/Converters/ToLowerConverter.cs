namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    /// <summary>
    /// Converts string values to lower case.
    /// </summary>
    public class ToLowerConverter : MarkupConverter<string, string>
    {
        protected override string Convert(string value, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            return value.ToLowerInvariant();
        }

        protected override string ConvertBack(string value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
