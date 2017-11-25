namespace Gu.Wpf.ModernUI.Demo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class MainViewModel : INotifyPropertyChanged
    {
        private string value = "Value from binding";

        private Uri welcomeSelected;

        private Uri layoutSelected;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Value
        {
            get => this.value;
            set
            {
                if (Equals(value, this.value))
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }

        public Uri WelcomeSelected
        {
            get => this.welcomeSelected;
            set
            {
                if (value == this.welcomeSelected)
                {
                    return;
                }

                this.welcomeSelected = value;
                this.OnPropertyChanged();
            }
        }

        public Uri LayoutSelected
        {
            get => this.layoutSelected;
            set
            {
                if (this.layoutSelected == value)
                {
                    return;
                }

                this.layoutSelected = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
