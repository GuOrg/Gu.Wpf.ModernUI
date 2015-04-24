namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Controls;

    using Gu.Wpf.ModernUI;

    /// <summary>
    /// Interaction logic for ControlsModernMenu.xaml
    /// </summary>
    public partial class ControlsModernMenu : UserControl
    {
        private int groupId = 2;
        private int linkId = 5;

        public ControlsModernMenu()
        {
            InitializeComponent();

            // add group command
            this.AddGroup.Command = new RelayCommand(o => {
                this.Menu.LinkGroups.Add(new LinkGroup {
                    DisplayName = string.Format(CultureInfo.InvariantCulture, "group {0}",
                    ++groupId)
                });
            });

            // add link to selected group command
            this.AddLink.Command = new RelayCommand(o => {
                this.Menu.SelectedLinkGroup.Links.Add(new Link {
                    DisplayName = string.Format(CultureInfo.InvariantCulture, "link {0}", ++linkId),
                    Source = new Uri(string.Format(CultureInfo.InvariantCulture, "/link{0}", linkId), UriKind.Relative)
                });
            }, o => this.Menu.SelectedLinkGroup != null);

            // remove selected group command
            this.RemoveGroup.Command = new RelayCommand(o => {
                this.Menu.LinkGroups.Remove(this.Menu.SelectedLinkGroup);
            }, o => this.Menu.SelectedLinkGroup != null);

            // remove selected linkcommand
            this.RemoveLink.Command = new RelayCommand(o => {
                this.Menu.SelectedLinkGroup.Links.Remove(SelectedLink(this.Menu));
            }, o => this.Menu.SelectedLinkGroup != null && SelectedLink(this.Menu) != null);

            // log SourceChanged events
            this.Menu.SelectedSourceChanged += (o, e) => {
                Debug.WriteLine("SelectedSourceChanged: {0}", e.Source);
            };
        }

        private static Link SelectedLink(ModernMenu menu)
        {
            return menu.SelectedLinkGroup.Links.FirstOrDefault(x => x.Source == menu.SelectedSource);
        }
    }
}
