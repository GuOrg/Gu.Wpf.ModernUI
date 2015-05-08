namespace Gu.Wpf.ModernUI
{
    using System.Windows;

    /// <summary>
    /// The links in a linkgroup
    /// </summary>
    public class LinkGroupLinks : ModernLinks
    {
        static LinkGroupLinks()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkGroupLinks), new FrameworkPropertyMetadata(typeof(LinkGroupLinks)));
        }
    }
}