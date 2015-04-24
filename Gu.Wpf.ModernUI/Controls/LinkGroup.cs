namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a named group of links.
    /// </summary>
    public class LinkGroup
        : ModernLinks, ILink
    {
        /// <summary>
        /// Identifies the DisplayName property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = Displayable.DisplayNameProperty.AddOwner(
            typeof(LinkGroup),
            new FrameworkPropertyMetadata(
                default(string),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the DisplayName property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = Link.SourceProperty.AddOwner(
            typeof(LinkGroup),
            new FrameworkPropertyMetadata(
                default(Uri),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(
            typeof(LinkGroup),
            new FrameworkPropertyMetadata(null));

        static LinkGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkGroup), new FrameworkPropertyMetadata(typeof(LinkGroup)));
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
        /// Gets or sets the source.
        /// </summary>
        /// <value>The display name.</value>
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Content
        {
            get
            {
                return (object)this.GetValue(ContentProperty);
            }
            set
            {
                this.SetValue(ContentProperty, value);
            }
        }
    }
}
