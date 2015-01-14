namespace Gu.ModernUI.Tests
{
    using System;

    using Gu.ModernUI.Navigation;

    using NUnit.Framework;

    public class DefaultLinkNavigatorTests
    {
        [TestCase(@"cmd:/largefontsize", UriKind.RelativeOrAbsolute, true)]
        [TestCase(@"cmd:/missing", UriKind.RelativeOrAbsolute,  false)]
        [TestCase(@"http://mui.codeplex.com/", UriKind.Absolute, true)]
        [TestCase(@"/ParentContent/1.xaml", UriKind.RelativeOrAbsolute, true)]
        public void CanNavigate(string uri, UriKind uriKind, bool expected)
        {
            var navigator = new DefaultLinkNavigator();
            var navigateTo = new Uri(uri, uriKind);
            Assert.AreEqual(expected, navigator.CanNavigate(navigateTo, null, null));
        }
    }
}
