namespace Gu.Wpf.ModernUI.Ninject.Demo
{
    using System.Windows;

    using global::Ninject;

    using Gu.Wpf.ModernUi.Ninject;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var kernel = new StandardKernel();
            kernel.Bind<ViewModel>()
                  .ToSelf()
                  .InSingletonScope();
            var ninjectLoader = new NinjectLoader(kernel);
            var mainWindow = kernel.Get<MainWindow>();
            mainWindow.ContentLoader = ninjectLoader;
            mainWindow.Show();
        }
    }
}
