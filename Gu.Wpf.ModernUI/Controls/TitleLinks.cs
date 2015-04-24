namespace Gu.Wpf.ModernUI
{
    using System.Windows;

    /// <summary>
    /// The links shown in the window border
    /// </summary>
    public class TitleLinks : ModernLinks
    {
        static TitleLinks()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleLinks), new FrameworkPropertyMetadata(typeof(TitleLinks)));
        }
    }
}