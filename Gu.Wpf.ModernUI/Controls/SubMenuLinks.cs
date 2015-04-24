namespace Gu.Wpf.ModernUI
{
    using System.Windows;

    /// <summary>
    /// The lower row of links in a menu
    /// </summary>
    public class SubMenuLinks : ModernLinks
    {
        static SubMenuLinks()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SubMenuLinks), new FrameworkPropertyMetadata(typeof(SubMenuLinks)));
        }
    }
}