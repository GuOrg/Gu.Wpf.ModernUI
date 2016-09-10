namespace Gu.Wpf.ModernUI.Ninject.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using JetBrains.Annotations;

    public class ViewModel1 : INotifyPropertyChanged
    {
        private string text;

        public ViewModel1(SharedViewModel shared)
        {
            this.Shared = shared;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SharedViewModel Shared { get; }

        public string Text
        {
            get
            {
                return this.text;
            }
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}