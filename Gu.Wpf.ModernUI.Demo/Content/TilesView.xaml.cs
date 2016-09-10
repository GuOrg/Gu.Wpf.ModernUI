namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for TilesView.xaml
    /// </summary>
    public partial class TilesView : UserControl
    {
        public TilesView()
        {
            var modernWindow = Application.Current.MainWindow as ModernWindow;
            if (modernWindow != null)
            {
                this.TileLinks = modernWindow.MainMenu.Links.Select(lg => new Link{DisplayName = lg.DisplayName, Source = lg.Source});
            }
            this.InitializeComponent();
        }

        public IEnumerable<Link> TileLinks { get; private set; }
    }
}
