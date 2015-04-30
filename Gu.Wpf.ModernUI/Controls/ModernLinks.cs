namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Internals;

    /// <summary>
    /// Base class for links
    /// </summary>
    public class ModernLinks : Control, INavigationSource
    {
        /// <summary>
        /// Identifies the Links dependency property.
        /// </summary>
        public static readonly DependencyProperty LinksProperty = DependencyProperty.Register(
            "Links",
            typeof(LinkCollection),
            typeof(ModernLinks),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Identifies the SelectedSource dependency property.
        /// </summary>
        internal static readonly DependencyPropertyKey SelectedLinkPropertyKey = DependencyProperty.RegisterReadOnly(
            "SelectedLink",
            typeof(Link),
            typeof(ModernLinks),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedLinkProperty = SelectedLinkPropertyKey.DependencyProperty;

        public static readonly DependencyProperty SelectedSourceProperty = DependencyProperty.Register(
            "SelectedSource",
            typeof(Uri),
            typeof(ModernLinks),
            new PropertyMetadata(default(Uri)));
       
        public static readonly DependencyProperty NavigationTargetProperty = Modern.NavigationTargetProperty.AddOwner(typeof(ModernLinks));

        private readonly NavigationSynchronizer synchronizer;

        static ModernLinks()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernLinks), new FrameworkPropertyMetadata(typeof(ModernLinks)));
        }

        /// <summary>
        /// 
        /// </summary>
        public ModernLinks()
        {
            this.synchronizer = NavigationSynchronizer.Create(this);
            SetCurrentValue(LinksProperty, new LinkCollection());
        }

        /// <summary>
        /// Gets or sets the collection of links that define the available content in this tab.
        /// </summary>
        public LinkCollection Links
        {
            get { return (LinkCollection)GetValue(LinksProperty); }
            set { SetValue(LinksProperty, value); }
        }

        /// <summary>
        /// Gets or sets the source URI of the selected link.
        /// </summary>
        /// <value>The source URI of the selected link.</value>
        public Link SelectedLink
        {
            get { return (Link)GetValue(SelectedLinkProperty); }
            set { SetValue(SelectedLinkPropertyKey, value); }
        }

        /// <summary>
        /// Get or sets the Uri of the selected Link
        /// </summary>
        public Uri SelectedSource
        {
            get { return (Uri)GetValue(SelectedSourceProperty); }
            set { SetValue(SelectedSourceProperty, value); }
        }
       
        /// <summary>
        /// Get or sets the target frame
        /// </summary>
        public ModernFrame NavigationTarget
        {
            get { return (ModernFrame)GetValue(NavigationTargetProperty); }
            set { SetValue(NavigationTargetProperty, value); }
        }

        public void Dispose()
        {
            this.synchronizer.Dispose();
        }
    }
}