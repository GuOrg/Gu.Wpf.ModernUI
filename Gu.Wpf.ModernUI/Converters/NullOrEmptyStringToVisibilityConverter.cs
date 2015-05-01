namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts a null or empty string value to Visibility.Visible and any other value to Visibility.Collapsed
    /// </summary>
    public class NullOrEmptyStringToVisibilityConverter : MarkupConverter<string, Visibility>
    {
        public Visibility WhenNullEmpty { get; set; }

        public Visibility Else { get; set; }

        protected override Visibility Convert(string value, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value)
                       ? this.WhenNullEmpty
                       : this.Else;
        }

        protected override string ConvertBack(Visibility value, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
