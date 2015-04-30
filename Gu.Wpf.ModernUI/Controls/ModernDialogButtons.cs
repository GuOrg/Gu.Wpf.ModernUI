namespace Gu.Wpf.ModernUI
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using ModernUi.Interfaces;

    public class ModernDialogButtons : Control
    {
        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(IEnumerable<DialogResult>), typeof(ModernDialogButtons), new PropertyMetadata(default(IEnumerable<DialogResult>)));
        static ModernDialogButtons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernDialogButtons), new FrameworkPropertyMetadata(typeof(ModernDialogButtons)));
        }

        public IEnumerable<DialogResult> Buttons
        {
            get
            {
                return (IEnumerable<DialogResult>)GetValue(ButtonsProperty);
            }
            set
            {
                SetValue(ButtonsProperty, value);
            }
        }
    }
}