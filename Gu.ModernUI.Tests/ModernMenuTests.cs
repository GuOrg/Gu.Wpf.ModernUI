namespace Gu.ModernUI.Tests
{
    using System;
    using System.Linq;

    using NUnit.Framework;
    [RequiresSTA]
    public class ModernMenuTests
    {
        private LinkGroup linkGroup1;

        private LinkGroup linkGroup2;

        private ModernMenu modernMenu;

        [SetUp]
        public void SetUp()
        {
            this.linkGroup1 = new LinkGroup();
            this.linkGroup1.Links.Add(new Link { DisplayName = "1_1", Source = new Uri("/1_1.xaml", UriKind.RelativeOrAbsolute) });
            this.linkGroup1.Links.Add(new Link { DisplayName = "1_2", Source = new Uri("/1_2.xaml", UriKind.RelativeOrAbsolute) });

            this.linkGroup2 = new LinkGroup();
            this.linkGroup2.Links.Add(new Link { DisplayName = "2_1", Source = new Uri("/2_1.xaml", UriKind.RelativeOrAbsolute) });
            this.linkGroup2.Links.Add(new Link { DisplayName = "2_2", Source = new Uri("/2_2.xaml", UriKind.RelativeOrAbsolute) });
            var linkGroupCollection = new LinkGroupCollection { this.linkGroup1, this.linkGroup2 };
            this.modernMenu = new ModernMenu
            {
                LinkGroups = linkGroupCollection
            };
        }
        [Test]
        public void SettingSelectedSourceUpdatesSelectedLinkgroup()
        {
            var selectedSource = this.linkGroup2.Links.Last().Source;
            this.modernMenu.SelectedSource = selectedSource;
            Assert.AreSame(this.linkGroup2, this.modernMenu.SelectedLinkGroup);
            Assert.AreEqual(selectedSource, this.linkGroup2.SelectedSource);
            Assert.AreEqual(selectedSource, this.modernMenu.SelectedSource);
        }

        [Test]
        public void SetSelectedSourceToFirstLinkDefault()
        {
            var expected = this.linkGroup1.Links.First().Source;
            this.modernMenu.SelectedLinkGroup = this.linkGroup1;
            Assert.AreEqual(expected, this.modernMenu.SelectedSource);
        }

        [Test]
        public void SetSelectedSourceToSelectedLink()
        {
            var selectedSource = this.linkGroup1.Links.Last()
                                           .Source;
            this.linkGroup1.SelectedSource = selectedSource;
            this.modernMenu.SelectedLinkGroup = this.linkGroup1;
            Assert.AreEqual(selectedSource, this.modernMenu.SelectedSource);
        }
    }
}
