namespace Gu.Wpf.ModernUI.Tests
{
    using System;

    using NUnit.Framework;

    [RequiresSTA]
    public class ModernTabTests
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
            var modernLinks = new ModernTab();
            var uri1 = new Uri("/1", UriKind.RelativeOrAbsolute);
            var link1 = new Link { DisplayName = "1", Source = uri1 };
            modernLinks.Links.Add(link1);
            modernLinks.Links.Add(new Link { DisplayName = "2", Source = new Uri("/2", UriKind.RelativeOrAbsolute) });
            Assert.AreEqual(uri1, modernLinks.SelectedSource);
            Assert.AreEqual(link1, modernLinks.SelectedLink);
        }
    }
}