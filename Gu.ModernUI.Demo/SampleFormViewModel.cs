namespace Gu.ModernUI.Demo
{
    using System.ComponentModel;

    using Gu.ModernUI.Presentation;

    public class SampleFormViewModel
        : NotifyPropertyChanged, IDataErrorInfo
    {
        private string firstName = "John";
        private string lastName;

        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                if (this.firstName != value) {
                    this.firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        public string LastName
        {
            get { return this.lastName; }
            set
            {
                if (this.lastName != value) {
                    this.lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "FirstName") {
                    return string.IsNullOrEmpty(this.firstName) ? "Required value" : null;
                }
                if (columnName == "LastName") {
                    return string.IsNullOrEmpty(this.lastName) ? "Required value" : null;
                }
                return null;
            }
        }
    }
}
