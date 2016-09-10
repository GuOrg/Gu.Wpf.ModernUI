namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>Converts a null value to Visibility.Visible and any other value to Visibility.Collapsed</summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class NullToVisibilityConverter : MarkupConverter<object, Visibility>
    {
        public NullToVisibilityConverter()
        {
        }

        /// <summary>The return value when the converted value is null.</summary>
        public Visibility WhenNull { get; set; }

        /// <summary>The return value when the converted value is not null.</summary>
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
