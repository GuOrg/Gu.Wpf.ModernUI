namespace Gu.Wpf.ModernUI.Tests
{
    using System;
    using System.Linq;
    using System.Windows.Media.Animation;

    using Gu.Wpf.ModernUI;
    using Gu.Wpf.ModernUI.Navigation;

    using Moq;

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
            this.linkGroup1 = new LinkGroup { DisplayName = "Group1" };
            this.linkGroup1.Links.Add(new Link { DisplayName = "1_1", Source = new Uri("/1_1.xaml", UriKind.RelativeOrAbsolute) });
            this.linkGroup1.Links.Add(new Link { DisplayName = "1_2", Source = new Uri("/1_2.xaml", UriKind.RelativeOrAbsolute) });

            this.linkGroup2 = new LinkGroup { DisplayName = "Group2" };
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
            Assert.IsTrue(this.linkGroup1.IsEnabled);
            Assert.IsFalse(this.linkGroup2.IsEnabled);
        }

        [Test]
        public void SetSelectedSourceToFirstLinkDefault()
        {
            var expected = this.linkGroup1.Links.First().Source;
            this.modernMenu.SelectedLinkGroup = this.linkGroup1;
            Assert.AreEqual(expected, this.modernMenu.SelectedSource);
            Assert.IsFalse(this.linkGroup1.IsEnabled);
            Assert.IsTrue(this.linkGroup2.IsEnabled);
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

        [Test]
        public void NavigatesIfSourceIsSet()
        {
            this.modernMenu.SelectedLinkGroup = this.linkGroup1;
            this.linkGroup2.Source = new Uri("http://mui.codeplex.com/");
            var linkNavigatorMock = new Mock<ILinkNavigator>();
            linkNavigatorMock.Setup(x => x.CanNavigate(It.IsAny<Uri>(), It.IsAny<Uri>(), null))
                             .Returns(true);
            this.modernMenu.LinkNavigator = linkNavigatorMock.Object;

            this.modernMenu.SelectedLinkGroup = this.linkGroup2;
            linkNavigatorMock.Verify(x => x.CanNavigate(this.linkGroup2.Source, this.linkGroup1.Links.First().Source, null), Times.Once);
            linkNavigatorMock.Verify(x => x.Navigate(this.linkGroup2.Source, It.IsAny<Action<Uri>>(), null), Times.Once);

            Assert.AreEqual(this.linkGroup1, this.modernMenu.SelectedLinkGroup);
            Assert.AreEqual(this.linkGroup1.SelectedSource, this.modernMenu.SelectedSource);
            Assert.IsFalse(this.linkGroup1.IsEnabled);
            Assert.IsTrue(this.linkGroup2.IsEnabled);
        }
    }
}
