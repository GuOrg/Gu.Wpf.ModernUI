namespace Gu.ModernUI.Tests
{
    using System;

    using Gu.ModernUI.Windows.Navigation;

    using NUnit.Framework;

    public class NavigationHelperTests
    {
        [TestCase(@"/Pages/LayoutWireframe.xaml", UriKind.Relative, null, @"/Pages/LayoutWireframe.xaml")]
        [TestCase(@"/Content/LoremIpsum.xaml#1", UriKind.Relative, "1", @"/Content/LoremIpsum.xaml")]
        public void RemoveFragment(string s, UriKind kind, string expectedFragment, string expectedUri)
        {
            string fragment;
            var uri = new Uri(s, kind);
            var removeFragment = NavigationHelper.RemoveFragment(uri, out fragment);
            Assert.AreEqual(expectedFragment, fragment);
            var expected = new Uri(expectedUri, kind);
            Assert.AreEqual(expected, removeFragment);
        }
    }
}
