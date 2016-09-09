namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Converts a null value to Visibility.Visible and any other value to Visibility.Collapsed
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class NullToVisibilityConverter : MarkupConverter<object, Visibility>
    {
        public NullToVisibilityConverter()
        {
        }

        public Visibility WhenNull { get; set; }

        public Visibility Else { get; set; }

        protected override Visibility Convert(object value, CultureInfo culture)
        {
            return value == null
                       ? this.WhenNull
                       : this.Else;
        }

        protected override object ConvertBack(Visibility value, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
