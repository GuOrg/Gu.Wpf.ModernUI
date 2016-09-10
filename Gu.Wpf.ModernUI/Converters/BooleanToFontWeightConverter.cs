namespace Gu.Wpf.ModernUI
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Converts a boolean value to a font weight (false: normal, true: bold)
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class BooleanToFontWeightConverter : MarkupConverter<bool?, FontWeight>
    {
        public BooleanToFontWeightConverter()
        {
        }

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is true
        /// </summary>
        public FontWeight WhenTrue { get; set; } = FontWeights.Bold;

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is false
        /// </summary>
        public FontWeight WhenFalse { get; set; } = FontWeights.Normal;

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is null
        /// </summary>
        public FontWeight WhenNull { get; set; } = FontWeights.Normal;

        protected override FontWeight Convert(bool? value, CultureInfo culture)
        {
            if (value == null)
            {
                return this.WhenNull;
            }
            return value == true ? this.WhenTrue : this.WhenFalse;
        }

        protected override bool? ConvertBack(FontWeight value, CultureInfo culture)
        {
            if (value == this.WhenTrue)
            {
                return true;
            }
            if (value == this.WhenFalse)
            {
                return false;
            }
            return null;
        }
    }
}
