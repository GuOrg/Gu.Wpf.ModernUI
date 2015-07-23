namespace Gu.Wpf.ModernUI.Ninject.Demo
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for View2.xaml
    /// </summary>
    public partial class View2 : UserControl
    {
        public View2(ViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
