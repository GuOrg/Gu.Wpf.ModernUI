namespace Gu.Wpf.ModernUI.Ninject.Demo
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for View2.xaml
    /// </summary>
    public partial class View2 : UserControl
    {
        public View2(ViewModel2 vm)
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }
    }
}
