namespace NavigationApp
{
    using System;
    using System.Linq;
    using System.Windows;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var windowName = e.Args.SingleOrDefault();
            if (windowName != null)
            {
                this.StartupUri = windowName == "MainWindow"
                      ? new Uri($"{windowName}.xaml", UriKind.Relative)
                      : new Uri($"UiTestWindows/{windowName}.xaml", UriKind.Relative);
            }


            base.OnStartup(e);
        }
    }
}
