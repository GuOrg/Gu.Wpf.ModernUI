namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class Link 
        : Button, ILink
    {
        /// <summary>
        /// Identifies the DisplayName property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = Displayable.DisplayNameProperty.AddOwner(
            typeof(Link),
            new FrameworkPropertyMetadata(
                default(string),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the SourceProperty property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(Uri),
            typeof(Link),
            new FrameworkPropertyMetadata(
                default(Uri),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        static Link()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Link), new FrameworkPropertyMetadata(typeof(Link)));
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the source uri.
        /// </summary>
        /// <value>The source.</value>
        public Uri Source
        {
            get
            {
                return (Uri)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }
    }
}
