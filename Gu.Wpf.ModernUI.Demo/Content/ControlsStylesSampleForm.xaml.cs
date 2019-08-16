namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Gu.ModernUI.Interfaces;

    using Gu.Wpf.ModernUI;
    using Gu.Wpf.ModernUI.Navigation;

    /// <summary>
    /// Interaction logic for ControlsStylesSampleForm.xaml
    /// </summary>
    public partial class ControlsStylesSampleForm : UserControl, INavigationView
    {
        private readonly SampleFormViewModel vm = new SampleFormViewModel();

        public ControlsStylesSampleForm()
        {
            this.InitializeComponent();
            this.DataContext = this.vm;
            this.Loaded += this.OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            // select first control on the form
            Keyboard.Focus(this.TextFirstName);
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (this.vm.IsDirty)
            {
                DefaultDialogHandler.Instance.Show("The form is dirty. Submit changes before navigating away.", "Message Dialog", MessageBoxButtons.OK);
            }

            e.Cancel = this.vm.IsDirty;
        }
    }
}
