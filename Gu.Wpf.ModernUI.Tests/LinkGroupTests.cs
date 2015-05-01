namespace Gu.Wpf.ModernUI.Tests
{
    using System;

    using NUnit.Framework;

    [RequiresSTA]
    public class LinkGroupTests
    {
        private Link link1;
        private Link link2;

        [SetUp]
        public void SetUp()
        {
            this.link1 = new Link { DisplayName = "1", Source = new Uri("/1", UriKind.RelativeOrAbsolute) };
            this.link2 = new Link { DisplayName = "2", Source = new Uri("/2", UriKind.RelativeOrAbsolute) };
        }

        [Test]
        public void InitializesWithDefaultSelected()
        {
            var linkGroup = new LinkGroup();
            var uri1 = new Uri("/1", UriKind.RelativeOrAbsolute);
            var link1 = new Link { DisplayName = "1", Source = uri1 };
            linkGroup.Links.Add(link1);
            linkGroup.Links.Add(new Link { DisplayName = "2", Source = new Uri("/2", UriKind.RelativeOrAbsolute) });
            Assert.AreEqual(uri1, linkGroup.SelectedSource);
            Assert.AreEqual(link1, linkGroup.SelectedLink);
        }

        [Test]
        public void SelectedLinkTracksSelectedSource()
        {
            var linkGroup = new LinkGroup();
            linkGroup.Links.Add(this.link1);
            linkGroup.Links.Add(this.link2);
            linkGroup.SelectedSource = this.link2.Source;
            Assert.AreEqual(this.link2, linkGroup.SelectedLink);
        }

        [Test]
        public void SelectedTracksNavigationTarget()
        {
            var linkGroup = new LinkGroup();
            var modernFrame = new ModernFrame();
            linkGroup.SetNavigationTarget(modernFrame);
            linkGroup.Links.Add(this.link1);
            linkGroup.Links.Add(this.link2);

            modernFrame.Source = this.link2.Source;
            Assert.AreEqual(this.link2.Source, linkGroup.SelectedSource);
            Assert.AreEqual(this.link2, linkGroup.SelectedLink);
        }

        [Test]
        public void SelectedSourceDoesNotUpdateWhenNavigationTargetNavigatesAway()
        {
            var linkGroup = new LinkGroup();
            var modernFrame = new ModernFrame();
            linkGroup.SetNavigationTarget(modernFrame);
            linkGroup.Links.Add(this.link1);
            linkGroup.Links.Add(this.link2);

            modernFrame.Source = this.link2.Source;
            Assert.AreEqual(this.link2.Source, linkGroup.SelectedSource);
            Assert.AreEqual(this.link2, linkGroup.SelectedLink);

            modernFrame.Source = new Uri("/3", UriKind.RelativeOrAbsolute);
            Assert.AreEqual(this.link2.Source, linkGroup.SelectedSource);
            Assert.AreEqual(this.link2, linkGroup.SelectedLink);
        }

        [Test]
        public void SelectedSourceDoesNotUpdateWhenNavigationTargetGoesNull()
        {
            var linkGroup = new LinkGroup();
            var modernFrame = new ModernFrame();
            linkGroup.SetNavigationTarget(modernFrame);
            linkGroup.Links.Add(this.link1);
            linkGroup.Links.Add(this.link2);

            modernFrame.Source = this.link2.Source;
            Assert.AreEqual(this.link2.Source, linkGroup.SelectedSource);
            Assert.AreEqual(this.link2, linkGroup.SelectedLink);

            linkGroup.SetNavigationTarget(modernFrame);
            Assert.AreEqual(this.link2.Source, linkGroup.SelectedSource);
            Assert.AreEqual(this.link2, linkGroup.SelectedLink);
        }

        [Test]
        public void IsNavigatedToTrue()
        {
            var linkGroup = new LinkGroup();
            var modernFrame = new ModernFrame();
            linkGroup.SetNavigationTarget(modernFrame);
            linkGroup.Links.Add(this.link1);
            linkGroup.Links.Add(this.link2);

            modernFrame.Source = this.link2.Source;
            Assert.IsTrue(linkGroup.IsNavigatedTo);
        }

        [Test]
        public void IsNavigatedToFalse()
        {
            var linkGroup = new LinkGroup();
            var modernFrame = new ModernFrame();
            linkGroup.SetNavigationTarget(modernFrame);
            linkGroup.Links.Add(this.link1);
            linkGroup.Links.Add(this.link2);

            modernFrame.Source = null;
            Assert.IsFalse(linkGroup.IsNavigatedTo);
        }
    }
}