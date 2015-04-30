namespace Gu.Wpf.ModernUI.Tests
{
    using System;

    using Gu.Wpf.ModernUI.Navigation;

    using NUnit.Framework;

    [RequiresSTA]
    public class DefaultLinkNavigatorTests
    {
        [TestCase(@"cmd:/largefontsize", UriKind.RelativeOrAbsolute, true)]
        [TestCase(@"cmd:/missing", UriKind.RelativeOrAbsolute, true, Description = "Must return true for this because there might be a custom contentloader")]
        [TestCase(@"http://mui.codeplex.com/", UriKind.Absolute, true)]
        [TestCase(@"/ParentContent/1.xaml", UriKind.RelativeOrAbsolute, true)]
        public void CanNavigate(string uri, UriKind kind, bool expected)
        {
            var navigator = new DefaultLinkNavigator();
            var navigateTo = new Uri(uri, kind);
            var frame = new ModernFrame();
            Assert.AreEqual(expected, navigator.CanNavigate(frame, navigateTo));
        }

        [TestCase(@"cmd:/largefontsize", UriKind.RelativeOrAbsolute, true)]
        [TestCase(@"http://mui.codeplex.com/", UriKind.Absolute, true)]
        [TestCase(@"/ParentContent/1.xaml", UriKind.RelativeOrAbsolute, false)]
        public void CanNavigateWhenNoFrame(string uri, UriKind kind, bool expected)
        {
            var navigator = new DefaultLinkNavigator();
            var navigateTo = new Uri(uri, kind);
            Assert.AreEqual(expected, navigator.CanNavigate(null, navigateTo));
        }
    }
}
