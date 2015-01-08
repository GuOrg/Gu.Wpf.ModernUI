using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace FirstFloor.ModernUI.App
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string value = "Value from binding";

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
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
