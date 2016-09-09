namespace Gu.Wpf.ModernUI
{
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Converts a boolean value to a font weight (false: normal, true: bold)
    /// </summary>
    public class BooleanToFontWeightConverter : MarkupConverter<bool?, FontWeight>
    {
        private FontWeight whenTrue = FontWeights.Bold;
        private FontWeight whenFalse = FontWeights.Normal;
        private FontWeight whenNull = FontWeights.Normal;

        public BooleanToFontWeightConverter()
        {
        }

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is true
        /// </summary>
        public FontWeight WhenTrue
        {
            get { return this.whenTrue; }
            set { this.whenTrue = value; }
        }

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is false
        /// </summary>
        public FontWeight WhenFalse
        {
            get { return this.whenFalse; }
            set { this.whenFalse = value; }
        }

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is null
        /// </summary>
        public FontWeight WhenNull
        {
            get { return this.whenNull; }
            set { this.whenNull = value; }
        }

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
