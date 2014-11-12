using System.Windows;
namespace FirstFloor.ModernUI.Presentation
{
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a named group of links.
    /// </summary>
    public class LinkGroup
        : ContentControl
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
        /// Identifies the GroupKey property.
        /// </summary>
        public static readonly DependencyProperty GroupKeyProperty = DependencyProperty.Register("GroupKey", typeof(string), typeof(LinkGroup), new PropertyMetadata(default(string)));

        private readonly LinkCollection links = new LinkCollection();
        private Link selectedLink;

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
        /// Gets or sets the key of the group.
        /// </summary>
        /// <value>The key of the group.</value>
        /// <remarks>
        /// The group key is used to group link groups in a <see cref="FirstFloor.ModernUI.Windows.Controls.ModernMenu"/>.
        /// </remarks>
        public string GroupKey
        {
            get
            {
                return (string)GetValue(GroupKeyProperty);
            }
            set
            {
                SetValue(GroupKeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected link in this group.
        /// </summary>
        /// <value>The selected link.</value>
        internal Link SelectedLink
        {
            get { return this.selectedLink; }
            set
            {
                if (this.selectedLink != value)
                {
                    this.selectedLink = value;
                    //OnPropertyChanged("SelectedLink");
                }
            }
        }

        /// <summary>
        /// Gets the links.
        /// </summary>
        /// <value>The links.</value>
        public LinkCollection Links
        {
            get { return this.links; }
        }
    }
}
