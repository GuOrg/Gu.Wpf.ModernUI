namespace Gu.Wpf.ModernUI.Ninject.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.ModernUI.Annotations;

    public class Person : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;

        public Person(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                if (value == this.firstName)
                {
                    return;
                }
                this.firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                if (value == this.lastName)
                {
                    return;
                }
                this.lastName = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
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