namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Xml.Linq;

    using ModernUI;

    /// <summary>
    /// Interaction logic for ControlsModernButton.xaml
    /// </summary>
    public partial class ControlsModernButton : UserControl
    {
        public ControlsModernButton()
        {
            this.InitializeComponent();

            // find all embedded XAML icon files
            var assembly = this.GetType().Assembly;
            var iconResourceNames = from name in assembly.GetManifestResourceNames()
                                    where name.StartsWith("Gu.Wpf.ModernUI.Demo.Assets.appbar.")
                                    select name;

            foreach (var name in iconResourceNames)
            {
                // load the resource stream
                using (var stream = assembly.GetManifestResourceStream(name))
                {
                    // parse the icon data using xml
                    var doc = XDocument.Load(stream);

                    var path = doc.Root?.Element("{http://schemas.microsoft.com/winfx/2006/xaml/presentation}Path");
                    if (path != null)
                    {
                        var data = (string)path.Attribute("Data");

                        // create a modern button and add it to the button panel
                        this.ButtonPanel.Children.Add(new ModernButton
                        {
                            IconData = Geometry.Parse(data),
                            Margin = new Thickness(0, 0, 8, 0)
                        });
                    }
                }
            }
        }
    }
}
