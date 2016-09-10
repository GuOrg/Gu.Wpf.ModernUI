namespace Gu.Wpf.ModernUI.Ninject.Demo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using JetBrains.Annotations;

    public class SharedViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Person> Persons { get; } = new ObservableCollection<Person>(new[]
        {
            new Person("Johan", "Larsson"),
            new Person("Lynn", "Crumbling"),
        });

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
