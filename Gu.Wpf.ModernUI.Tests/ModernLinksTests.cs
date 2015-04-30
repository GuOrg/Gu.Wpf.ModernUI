namespace Gu.Wpf.ModernUI.Tests
{
    using System;
    using NUnit.Framework;

    [RequiresSTA]
    public class ModernLinksTests
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
            var modernLinks = new ModernLinks();
            var uri1 = new Uri("/1", UriKind.RelativeOrAbsolute);
            var link1 = new Link { DisplayName = "1", Source = uri1 };
            modernLinks.Links.Add(link1);
            modernLinks.Links.Add(new Link { DisplayName = "2", Source = new Uri("/2", UriKind.RelativeOrAbsolute) });
            Assert.AreEqual(uri1, modernLinks.SelectedSource);
            Assert.AreEqual(link1, modernLinks.SelectedLink);
        }

        [Test]
        public void SelectedLinkTracksSelectedSource()
        {
            var modernLinks = new ModernLinks();
            modernLinks.Links.Add(link1);
            modernLinks.Links.Add(link2);
            modernLinks.SelectedSource = link2.Source;
            Assert.AreEqual(link2, modernLinks.SelectedLink);
        }

        [Test]
        public void SelectedTracksNavigationTarget()
        {
            var modernLinks = new ModernLinks();
            var modernFrame = new ModernFrame();
            modernLinks.SetNavigationTarget(modernFrame);
            modernLinks.Links.Add(link1);
            modernLinks.Links.Add(link2);

            modernFrame.Source = link2.Source;
            Assert.AreEqual(link2.Source, modernLinks.SelectedSource);
            Assert.AreEqual(link2, modernLinks.SelectedLink);
        }

        [Test]
        public void SelectedSourceDoesNotUpdateWhenNavigationTargetNavigatesAway()
        {
            var modernLinks = new ModernLinks();
            var modernFrame = new ModernFrame();
            modernLinks.SetNavigationTarget(modernFrame);
            modernLinks.Links.Add(link1);
            modernLinks.Links.Add(link2);

            modernFrame.Source = link2.Source;
            Assert.AreEqual(link2.Source, modernLinks.SelectedSource);
            Assert.AreEqual(link2, modernLinks.SelectedLink);

            modernFrame.Source = new Uri("/3", UriKind.RelativeOrAbsolute);
            Assert.AreEqual(link2.Source, modernLinks.SelectedSource);
            Assert.AreEqual(link2, modernLinks.SelectedLink);
        }

        [Test]
        public void SelectedSourceDoesNotUpdateWhenNavigationTargetGoesNull()
        {
            var modernLinks = new ModernLinks();
            var modernFrame = new ModernFrame();
            modernLinks.SetNavigationTarget(modernFrame);
            modernLinks.Links.Add(link1);
            modernLinks.Links.Add(link2);

            modernFrame.Source = link2.Source;
            Assert.AreEqual(link2.Source, modernLinks.SelectedSource);
            Assert.AreEqual(link2, modernLinks.SelectedLink);

            modernLinks.SetNavigationTarget(modernFrame);
            Assert.AreEqual(link2.Source, modernLinks.SelectedSource);
            Assert.AreEqual(link2, modernLinks.SelectedLink);
        }
    }
}