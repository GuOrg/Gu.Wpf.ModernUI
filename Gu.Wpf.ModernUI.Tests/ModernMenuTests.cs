namespace Gu.Wpf.ModernUI.Tests
{
    using System;
    using System.Linq;
    using System.Threading;
    using Moq;

    using NUnit.Framework;

    [RequiresSTA]
    public class ModernMenuTests
    {
        private LinkGroup linkGroup1;
        private LinkGroup linkGroup2;
        private ModernMenu modernMenu;

        private Mock<IContentLoader> contentLoaderMock;

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
                LinkGroups = linkGroupCollection,
            };
            this.contentLoaderMock = new Mock<IContentLoader>(MockBehavior.Strict);
        }

        [Test]
        public void TracksNavigationTarget()
        {
            var frame = new ModernFrame
                            {
                                ContentLoader = this.contentLoaderMock.Object
                            };
            this.modernMenu.NavigationTarget = frame;
            var link = this.linkGroup2.Links.Last();
            frame.Source = link.Source;
            Assert.AreEqual(link.Source, this.modernMenu.SelectedSource);
            Assert.AreEqual(link, this.modernMenu.SelectedLink);
            Assert.AreEqual(this.linkGroup2, this.modernMenu.SelectedLinkGroup);
            this.contentLoaderMock.Verify(x => x.LoadContentAsync(link.Source, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
