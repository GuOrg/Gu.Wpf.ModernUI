namespace Gu.ModernUI.Presentation
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The base implementation of the INotifyPropertyChanged contract.
    /// </summary>
    public abstract class NotifyPropertyChanged
        : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
