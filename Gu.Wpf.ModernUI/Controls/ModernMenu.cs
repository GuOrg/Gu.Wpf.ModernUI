namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Internals;

    /// <summary>
    /// Represents the menu in a Modern UI styled window.
    /// </summary>
    public class ModernMenu : Control, IDisposable
    {
        /// <summary>
        /// Defines the LinkGroups dependency property.
        /// </summary>
        public static readonly DependencyProperty LinkGroupsProperty = DependencyProperty.Register(
            "LinkGroups",
            typeof(LinkGroupCollection), 
            typeof(ModernMenu), 
            new PropertyMetadata());

        /// <summary>
        /// Defines the SelectedLinkGroup dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedLinkGroupProperty = DependencyProperty.Register(
            "SelectedLinkGroup",
            typeof(LinkGroup), 
            typeof(ModernMenu),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedLinkProperty = ModernLinks.SelectedLinkProperty.AddOwner(typeof(ModernMenu));
        public static readonly DependencyProperty SelectedSourceProperty = ModernLinks.SelectedSourceProperty.AddOwner(typeof(ModernMenu));
        public static readonly DependencyProperty NavigationTargetProperty = Modern.NavigationTargetProperty.AddOwner(typeof(ModernMenu));
        private readonly NavigationTargetSourceChangedListener sourceChangedListener;

        static ModernMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernMenu), new FrameworkPropertyMetadata(typeof(ModernMenu)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernMenu"/> class.
        /// </summary>
        public ModernMenu()
        {
            this.sourceChangedListener = new NavigationTargetSourceChangedListener(this);
            this.sourceChangedListener.Changed += OnSourceChanged;
            SetCurrentValue(LinkGroupsProperty, new LinkGroupCollection());
        }

        /// <summary>
        /// Gets or sets the link groups.
        /// </summary>
        /// <value>The link groups.</value>
        public LinkGroupCollection LinkGroups
        {
            get { return (LinkGroupCollection)GetValue(LinkGroupsProperty); }
            set { SetValue(LinkGroupsProperty, value); }
        }

        /// <summary>
        /// Gets the selected link groups.
        /// </summary>
        public LinkGroup SelectedLinkGroup
        {
            get { return (LinkGroup)GetValue(SelectedLinkGroupProperty); }
            set { SetValue(SelectedLinkGroupProperty, value); }
        }

        /// <summary>
        /// Gets or sets the source URI of the selected link.
        /// </summary>
        /// <value>The source URI of the selected link.</value>
        public Link SelectedLink
        {
            get { return (Link)GetValue(SelectedLinkProperty); }
            protected set { SetValue(ModernLinks.SelectedLinkPropertyKey, value); }
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
            this.sourceChangedListener.Dispose();
        }

        private void OnSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var navigationTarget = this.GetNavigationTarget();
            if (navigationTarget == null || this.LinkGroups == null)
            {
                return;
            }
            var match = this.LinkGroups.Where(lg => lg.Links != null)
                            .FirstOrDefault(x => x.Links.Any(l => Equals(navigationTarget.Source, l.Source)));
            if (match != null)
            {
                this.SelectedLinkGroup = match;
                this.SelectedSource = navigationTarget.Source;
                this.SelectedLink = match.SelectedLink;
            }
        }
    }
}
