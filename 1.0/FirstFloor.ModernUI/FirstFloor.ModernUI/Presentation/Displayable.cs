using System.Windows;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace FirstFloor.ModernUI.Presentation
{
    /// <summary>
    /// Provides a base implementation for objects that are displayed in the UI.
    /// </summary>
    public abstract class Displayable
        : FrameworkElement, INotifyPropertyChanged
    {
        /// <summary>
        /// Identifies the DisplayNameProperty property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
            "DisplayName",
            typeof(string),
            typeof(Displayable),
            new FrameworkPropertyMetadata(
                default(string),
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
#if NET4
        protected virtual void OnPropertyChanged(string propertyName)
#else
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
#endif        
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
