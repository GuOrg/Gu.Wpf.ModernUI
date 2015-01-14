namespace Gu.ModernUI.Tests
{
    using System;

    using NUnit.Framework;
    [RequiresSTA]
    public class ModernMenuTests
    {
        [Test]
        public void SettingSelectedSourceUpdatesSelectedLinkgroup()
        {
            var linkGroup1 = new LinkGroup();
            linkGroup1.Links.Add(new Link { DisplayName = "1_1", Source = new Uri("/1_1.xaml", UriKind.RelativeOrAbsolute) });
            linkGroup1.Links.Add(new Link { DisplayName = "1_2", Source = new Uri("/1_2.xaml", UriKind.RelativeOrAbsolute) });

            var linkGroup2 = new LinkGroup();
            linkGroup2.Links.Add(new Link { DisplayName = "2_1", Source = new Uri("/2_1.xaml", UriKind.RelativeOrAbsolute) });
            linkGroup2.Links.Add(new Link { DisplayName = "2_2", Source = new Uri("/2_2.xaml", UriKind.RelativeOrAbsolute) });
            var linkGroupCollection = new LinkGroupCollection { linkGroup1, linkGroup2 };
            var modernMenu = new ModernMenu
                                 {
                                     LinkGroups = linkGroupCollection
                                 };

            var selectedSource = new Uri("/2_2.xaml", UriKind.RelativeOrAbsolute);
            modernMenu.SelectedSource = selectedSource;
            Assert.AreSame(linkGroup2, modernMenu.SelectedLinkGroup);
            Assert.AreEqual(selectedSource, linkGroup2.Source);
            Assert.AreEqual(selectedSource, modernMenu.SelectedSource);
        }
    }
}
