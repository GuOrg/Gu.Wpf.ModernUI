namespace Gu.ModernUI.Windows.Controls
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    using Gu.ModernUI.Presentation;

    /// <summary>
    /// Represents a control that contains multiple pages that share the same space on screen.
    /// </summary>
    [ContentProperty("Links")]
    public class ModernTab
        : Control
    {
        /// <summary>
        /// Identifies the ContentLoader dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentLoaderProperty = DependencyProperty.Register("ContentLoader", typeof(IContentLoader), typeof(ModernTab), new PropertyMetadata(new DefaultContentLoader()));
        /// <summary>
        /// Identifies the Layout dependency property.
        /// </summary>
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register("Layout", typeof(TabLayout), typeof(ModernTab), new PropertyMetadata(TabLayout.Tab));
        /// <summary>
        /// Identifies the ListWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ListWidthProperty = DependencyProperty.Register("ListWidth", typeof(GridLength), typeof(ModernTab), new PropertyMetadata(new GridLength(170)));
        /// <summary>
        /// Identifies the Links dependency property.
        /// </summary>
        public static readonly DependencyProperty LinksProperty = DependencyProperty.Register("Links", typeof(LinkCollection), typeof(ModernTab), new PropertyMetadata(new LinkCollection()));
        /// <summary>
        /// Identifies the SelectedSource dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedSourceProperty = DependencyProperty.Register("SelectedSource", typeof(Uri), typeof(ModernTab), new PropertyMetadata(OnSelectedSourceChanged));

        /// <summary>
        /// Occurs when the selected source has changed.
        /// </summary>
        public event EventHandler<SourceEventArgs> SelectedSourceChanged;

        static ModernTab()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernTab), new FrameworkPropertyMetadata(typeof(ModernTab)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernTab"/> control.
        /// </summary>
        public ModernTab()
        {
            // create a default links collection
            SetCurrentValue(LinksProperty, new LinkCollection());
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
        /// Gets or sets a value indicating how the tab should be rendered.
        /// </summary>
        public TabLayout Layout
        {
            get { return (TabLayout)GetValue(LayoutProperty); }
            set { SetValue(LayoutProperty, value); }
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
        /// Gets or sets the width of the list when Layout is set to List.
        /// </summary>
        /// <value>
        /// The width of the list.
        /// </value>
        public GridLength ListWidth
        {
            get { return (GridLength)GetValue(ListWidthProperty); }
            set { SetValue(ListWidthProperty, value); }
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
            ((ModernTab)o).OnSelectedSourceChanged((Uri)e.OldValue, (Uri)e.NewValue);
        }
    }
}
