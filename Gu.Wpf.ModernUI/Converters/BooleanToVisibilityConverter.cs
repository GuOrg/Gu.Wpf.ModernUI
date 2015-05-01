namespace Gu.Wpf.ModernUI
{
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Converts boolean to visibility values.
    /// </summary>
    public class BooleanToVisibilityConverter: MarkupConverter<bool?, Visibility>
    {
        private Visibility whenTrue = Visibility.Visible;
        private Visibility whenFalse = Visibility.Collapsed;
        private Visibility whenNull = Visibility.Collapsed;

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is true
        /// </summary>
        public Visibility WhenTrue
        {
            get
            {
                return this.whenTrue;
            }
            set
            {
                this.whenTrue = value;
            }
        }

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is false
        /// </summary>
        public Visibility WhenFalse
        {
            get
            {
                return this.whenFalse;
            }
            set
            {
                this.whenFalse = value;
            }
        }

        /// <summary>
        /// Gets or sets the value to be returned when the converted value is null
        /// </summary>
        public Visibility WhenNull
        {
            get
            {
                return this.whenNull;
            }
            set
            {
                this.whenNull = value;
            }
        }

        protected override Visibility Convert(bool? value, CultureInfo culture)
        {
            if (value == null)
            {
                return this.WhenNull;
            }
            return value == true ? this.WhenTrue : this.WhenFalse;
        }

        protected override bool? ConvertBack(Visibility value, CultureInfo culture)
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
