namespace Gu.Wpf.ModernUI
{
    using System.Windows;

    /// <summary>
    /// The links shown in a ModernTab
    /// </summary>
    public class TabLinks : ModernLinks
    {
        static TabLinks()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabLinks), new FrameworkPropertyMetadata(typeof(TabLinks)));
        }
    }
}