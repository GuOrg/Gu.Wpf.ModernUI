﻿namespace Gu.Wpf.ModernUI
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Converts a boolean value to a font weight (false: normal, true: bold)
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Used from xaml")]
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class BooleanToFontWeightConverter : MarkupConverter<bool?, FontWeight>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToFontWeightConverter"/> class.
        /// </summary>
        // ReSharper disable once EmptyConstructor, think xaml needs it
        public BooleanToFontWeightConverter()
        {
        }

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is true
        /// <remarks>
        /// Default value FontWeights.Bold
        /// </remarks>
        /// </summary>
        public FontWeight WhenTrue { get; set; } = FontWeights.Bold;

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is false
        /// <remarks>
        /// Default value FontWeights.Normal
        /// </remarks>
        /// </summary>
        public FontWeight WhenFalse { get; set; } = FontWeights.Normal;

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is null
        /// <remarks>
        /// Default value FontWeights.Normal
        /// </remarks>
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
