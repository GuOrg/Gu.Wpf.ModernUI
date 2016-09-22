namespace NavigationApp
{
    using System;
    using System.Windows.Input;

    public partial class JulienReproWindow
    {
        public JulienReproWindow()
        {
            this.DataContext = this;
            this.CmdGoToPage = new RelayCommand(this.GoToPage);
        }

        public ICommand CmdGoToPage { get; }

        private void GoToPage(object o)
        {
            this.LinkNavigator.Navigate(this.NavigationTarget, (Uri)o);
        }
    }
}
