namespace Gu.Wpf.ModernUI.Ninject.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ViewModel2 : INotifyPropertyChanged
    {
        private string text;

        public ViewModel2(SharedViewModel shared)
        {
            this.Shared = shared;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SharedViewModel Shared { get; }

        public string Text
        {
            get => this.text;

            set
            {
                if (value == this.text)
                {
                    return;
                }

                this.text = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}