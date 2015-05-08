namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Converts a null or empty string value to Visibility.Visible and any other value to Visibility.Collapsed
    /// </summary>
    public class NullOrEmptyStringToVisibilityConverter : MarkupConverter<string, Visibility>
    {
        /// <summary>
        /// Exposing explicit 
        /// </summary>
        public NullOrEmptyStringToVisibilityConverter()
        {
        }

        public Visibility WhenNullOrEmpty { get; set; }

        public Visibility Else { get; set; }

        protected override Visibility Convert(string value, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value)
                       ? this.WhenNullOrEmpty
                       : this.Else;
        }

        protected override string ConvertBack(Visibility value, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
