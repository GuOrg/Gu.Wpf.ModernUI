namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Controls;

    using ModernUI;

    /// <summary>
    /// Interaction logic for ControlsModernMenu.xaml
    /// </summary>
    public partial class ControlsModernMenu : UserControl
    {
        public ControlsModernMenu()
        {
            InitializeComponent();

            // add group command
            //this.AddGroup.Command = new RelayCommand(_ => this.Menu.LinkGroups.Add(new LinkGroup { DisplayName = string.Format("group {0}", (this.Menu.LinkGroups.Count + 1)) }));

            //// add link to selected group command
            //this.AddLink.Command = new RelayCommand(
            //    _ =>
            //    {
            //        var id = this.Menu.SelectedLinkGroup.Links.Count + 1;
            //        this.Menu.SelectedLinkGroup.Links.Add(new Link
            //        {
            //            DisplayName = string.Format("link {0}-{1}", id, this.Menu.SelectedLinkGroup.DisplayName),
            //            Source = new Uri(string.Format("/link{0}-{1}", id, this.Menu.SelectedLinkGroup.DisplayName), UriKind.Relative)
            //        });
            //    },
            //_ => this.Menu.SelectedLinkGroup != null);

            //// remove selected group command
            //this.RemoveGroup.Command = new RelayCommand(
            //    _ => this.Menu.LinkGroups.Remove(this.Menu.SelectedLinkGroup),
            //    _ => this.Menu.SelectedLinkGroup != null);

            //// remove selected linkcommand
            //this.RemoveLink.Command = new RelayCommand(o =>
            //{
            //    this.Menu.SelectedLinkGroup.Links.Remove(SelectedLink(this.Menu));
            //}, o => this.Menu.SelectedLinkGroup != null && SelectedLink(this.Menu) != null);

            //// log SourceChanged events
            //this.Menu.SelectedSourceChanged += (o, e) => {
            //    Debug.WriteLine("SelectedSourceChanged: {0}", e.Source);
            //};
        }

        private static Link SelectedLink(ModernMenu menu)
        {
            var target = menu.GetNavigationTarget();
            if (target == null)
            {
                return null;
            }
            return menu.SelectedLinkGroup.Links.Items.OfType<Link>()
                       .FirstOrDefault(x => x.Source == target.CurrentSource);
        }
    }
}
