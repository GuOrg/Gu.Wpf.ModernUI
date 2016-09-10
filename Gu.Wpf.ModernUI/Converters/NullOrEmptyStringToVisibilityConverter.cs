namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Converts a null or empty string value to Visibility.Visible and any other value to Visibility.Collapsed
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class NullOrEmptyStringToVisibilityConverter : MarkupConverter<string, Visibility>
    {
        /// <summary>For use in xaml via {x:Static mui:ToLowerConverter.Default}</summary>
        public static readonly NullOrEmptyStringToVisibilityConverter Default = new NullOrEmptyStringToVisibilityConverter();

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
