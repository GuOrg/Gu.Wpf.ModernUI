namespace Gu.Wpf.ModernUI.Ninject.Demo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.ModernUI.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Person> persons = new ObservableCollection<Person>(new[]{new Person("Johan","Larsson"),new Person("Lynn","Crumbling"),  });

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Person> Persons
        {
            get
            {
                return this.persons;
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
