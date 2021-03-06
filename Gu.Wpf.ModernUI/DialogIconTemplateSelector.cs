namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    using Gu.ModernUI.Interfaces;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Used from xaml")]
    public class DialogIconTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NoneTemplate { get; set; }

        public DataTemplate ErrorTemplate { get; set; }

        public DataTemplate QuestionTemplate { get; set; }

        public DataTemplate ExclamationTemplate { get; set; }

        public DataTemplate InformationTemplate { get; set; }

        /// <inheritdoc />
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var icon = item as MessageBoxIcon?;
            if (!icon.HasValue)
            {
                return this.NoneTemplate;
            }

            switch (icon.Value)
            {
                case MessageBoxIcon.None:
                    return this.NoneTemplate;
                case MessageBoxIcon.Error:
                    return this.ErrorTemplate;
                case MessageBoxIcon.Question:
                    return this.QuestionTemplate;
                case MessageBoxIcon.Exclamation:
                    return this.ExclamationTemplate;
                case MessageBoxIcon.Information:
                    return this.InformationTemplate;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}