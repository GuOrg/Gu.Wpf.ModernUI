namespace Gu.Wpf.ModernUI.Demo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class MainViewModel : INotifyPropertyChanged
    {
        private string value = "Value from binding";
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ShowDialogCommand { get; private set; }

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (!Equals(value, this.value))
                {
                    return;
                }
                this.value = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
