namespace Gu.Wpf.ModernUI.Ninject.Demo
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class View1 : UserControl
    {
        public View1(ViewModel1 vm)
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }
    }
}
