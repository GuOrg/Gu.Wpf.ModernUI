namespace Gu.Wpf.ModernUI.Demo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.ModernUI.Demo.Annotations;

    public class MainViewModel : INotifyPropertyChanged
    {
        private string value = "Value from binding";

        private Uri welcomeSelected;

        private Uri layoutSelected;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public Uri WelcomeSelected
        {
            get
            {
                return this.welcomeSelected;
            }
            set
            {
                if (value == this.welcomeSelected)
                {
                    return;
                }
                this.welcomeSelected = value;
                OnPropertyChanged();
            }
        }

        public Uri LayoutSelected
        {
            get
            {
                return this.layoutSelected;
            }
            set
            {
                if (this.layoutSelected == value)
                {
                    return;
                }
                this.layoutSelected = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
