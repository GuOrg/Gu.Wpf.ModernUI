namespace Gu.Wpf.ModernUI
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A control for showing messages ribbon style
    /// </summary>
    public class RibbonDialog : ContentControl
    {
        static RibbonDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonDialog), new FrameworkPropertyMetadata(typeof(RibbonDialog)));
        }
    }
}
