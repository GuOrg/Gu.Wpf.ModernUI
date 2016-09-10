namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    using Gu.ModernUI.Interfaces;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DialogButtonTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OKTemplate { get; set; }
      
        public DataTemplate CancelTemplate { get; set; }
        
        public DataTemplate AbortTemplate { get; set; }
        
        public DataTemplate RetryTemplate { get; set; }
        
        public DataTemplate IgnoreTemplate { get; set; }
        
        public DataTemplate YesTemplate { get; set; }
        
        public DataTemplate NoTemplate { get; set; }

        /// <inheritdoc />
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var result = item as DialogResult?;
            if (!result.HasValue)
            {
                return base.SelectTemplate(item, container);
            }

            switch (result.Value)
            {
                case DialogResult.None:
                    return base.SelectTemplate(item, container);
                case DialogResult.OK:
                    return this.OKTemplate;
                case DialogResult.Cancel:
                    return this.CancelTemplate;
                case DialogResult.Abort:
                    return this.AbortTemplate;
                case DialogResult.Retry:
                    return this.RetryTemplate;
                case DialogResult.Ignore:
                    return this.IgnoreTemplate;
                case DialogResult.Yes:
                    return this.YesTemplate;
                case DialogResult.No:
                    return this.NoTemplate;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
