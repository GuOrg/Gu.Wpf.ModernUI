namespace Gu.ModernUI.Windows.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    using Gu.ModernUI.Presentation;
    using Gu.ModernUI.Windows.Navigation;

    /// <summary>
    /// Base class for links
    /// </summary>
    [ContentProperty("Links")]
    public abstract class ModernLinks : Control
    {
        /// <summary>
        /// Identifies the Links dependency property.
        /// </summary>
        public static readonly DependencyProperty LinksProperty = DependencyProperty.Register("Links", typeof(LinkCollection), typeof(ModernLinks), new PropertyMetadata());

        /// <summary>
        /// Identifies the SelectedSource dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedSourceProperty = DependencyProperty.Register("SelectedSource", typeof(Uri), typeof(ModernLinks), new PropertyMetadata(null, OnSelectedSourceChanged, CoerceSelectedSourceChanged));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ContentLoaderProperty = ModernFrame.ContentLoaderProperty.AddOwner(typeof(ModernLinks));

        /// <summary>
        /// Identifies the LinkNavigator dependency property.
        /// </summary>
        public static DependencyProperty LinkNavigatorProperty = ModernFrame.LinkNavigatorProperty.AddOwner(typeof(ModernLinks));

        private bool isNavigating;

        /// <summary>
        /// 
        /// </summary>
        protected ModernLinks()
        {
            this.Links = new LinkCollection();
        }

        /// <summary>
        /// Occurs when the selected source has changed.
        /// </summary>
        public event EventHandler<SourceEventArgs> SelectedSourceChanged;

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
        public Uri SelectedSource
        {
            get { return (Uri)GetValue(SelectedSourceProperty); }
            set { SetValue(SelectedSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content loader.
        /// </summary>
        public IContentLoader ContentLoader
        {
            get { return (IContentLoader)GetValue(ContentLoaderProperty); }
            set { SetValue(ContentLoaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the link navigator.
        /// </summary>
        /// <value>The link navigator.</value>
        public ILinkNavigator LinkNavigator
        {
            get { return (ILinkNavigator)GetValue(LinkNavigatorProperty); }
            set { SetValue(LinkNavigatorProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnSelectedSourceChanged(Uri oldValue, Uri newValue)
        {
            // raise SelectedSourceChanged event
            var handler = this.SelectedSourceChanged;
            if (handler != null)
            {
                handler(this, new SourceEventArgs(newValue));
            }
        }

        private static void OnSelectedSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!Equals(e.OldValue, e.NewValue))
            {
                ((ModernLinks)o).OnSelectedSourceChanged((Uri)e.OldValue, (Uri)e.NewValue);
            }
        }

        private static object CoerceSelectedSourceChanged(DependencyObject o, object basevalue)
        {
            var modernLinks = (ModernLinks)o;
            if (!modernLinks.isNavigating)
            {
                modernLinks.Navigate(basevalue as Uri);
            }
            return modernLinks.SelectedSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUri"></param>
        protected virtual void Navigate(Uri newUri)
        {
            if (this.LinkNavigator == null)
            {
                return;
            }
            if (this.LinkNavigator.CanNavigate(newUri, this.SelectedSource, null))
            {
                this.isNavigating = true;
                this.LinkNavigator.Navigate(newUri, x => this.SelectedSource = x, null);
                this.isNavigating = false;
            }
        }
    }
}