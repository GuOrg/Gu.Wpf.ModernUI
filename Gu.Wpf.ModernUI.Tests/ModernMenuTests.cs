namespace Gu.Wpf.ModernUI.Tests
{
    using System;
    using System.Linq;

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
    }
}
