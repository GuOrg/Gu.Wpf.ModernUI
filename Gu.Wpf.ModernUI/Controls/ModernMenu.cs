namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Gu.Wpf.ModernUI.Navigation;

    /// <summary>
    /// Represents the menu in a Modern UI styled window.
    /// </summary>
    public class ModernMenu
        : Control
    {
        /// <summary>
        /// Defines the LinkGroups dependency property.
        /// </summary>
        public static readonly DependencyProperty LinkGroupsProperty = DependencyProperty.Register("LinkGroups", typeof(LinkGroupCollection), typeof(ModernMenu), new PropertyMetadata());
        /// <summary>
        /// Defines the SelectedLinkGroup dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedLinkGroupProperty = DependencyProperty.Register("SelectedLinkGroup", typeof(LinkGroup), typeof(ModernMenu),
            new PropertyMetadata(
                null,
                OnSelectedLinkGroupChanged,
                CoerceSelectedLinkGroup));

        /// <summary>
        /// Defines the SelectedSource dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedSourceProperty = DependencyProperty.Register("SelectedSource", typeof(Uri), typeof(ModernMenu), new PropertyMetadata(OnSelectedSourceChanged));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LinkNavigatorProperty = ModernFrame.LinkNavigatorProperty.AddOwner(typeof(ModernMenu));

        /// <summary>
        /// Occurs when the selected source has changed.
        /// </summary>
        public event EventHandler<SourceEventArgs> SelectedSourceChanged;

        static ModernMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernMenu), new FrameworkPropertyMetadata(typeof(ModernMenu)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernMenu"/> class.
        /// </summary>
        public ModernMenu()
        {
            // create a default link groups collection
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
        public Uri SelectedSource
        {
            get { return (Uri)GetValue(SelectedSourceProperty); }
            set { SetValue(SelectedSourceProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILinkNavigator LinkNavigator
        {
            get
            {
                return (ILinkNavigator)this.GetValue(LinkNavigatorProperty);
            }
            set
            {
                this.SetValue(LinkNavigatorProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnSelectedSourceChanged(Uri oldValue, Uri newValue)
        {
            if (newValue != null && this.LinkGroups != null)
            {
                var linkGroup = this.LinkGroups.FirstOrDefault(x => x.Links.Any(l => l.Source == newValue));
                if (linkGroup != null)
                {
                    if (!Equals(linkGroup.SelectedSource, newValue))
                    {
                        linkGroup.SelectedSource = linkGroup.Links.First(x => x.Source == newValue)
                                                                  .Source;
                    }

                    if (!Equals(this.SelectedLinkGroup, linkGroup))
                    {
                        SetCurrentValue(SelectedLinkGroupProperty, linkGroup);
                    }
                }

            }
            // raise SelectedSourceChanged event
            var handler = this.SelectedSourceChanged;
            if (handler != null)
            {
                handler(this, new SourceEventArgs(newValue));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnSelectedLinkGroupChanged(LinkGroup oldValue, LinkGroup newValue)
        {
            if (newValue != null)
            {
                newValue.IsEnabled = false;
                if (newValue.SelectedSource == null)
                {
                    var firstOrDefault = newValue.Links.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        newValue.SelectedSource = firstOrDefault.Source;
                    }
                }
                if (!Equals(this.SelectedSource, newValue.SelectedSource))
                {
                    SetCurrentValue(SelectedSourceProperty, newValue.SelectedSource);
                }

            }
            else
            {
                SetCurrentValue(SelectedSourceProperty, null);
            }
            if (oldValue != null)
            {
                oldValue.IsEnabled = true;
            }
        }

        private static void OnSelectedLinkGroupChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!Equals(e.OldValue, e.NewValue))
            {
                ((ModernMenu)o).OnSelectedLinkGroupChanged((LinkGroup)e.OldValue, (LinkGroup)e.NewValue);
            }
        }

        private static object CoerceSelectedLinkGroup(DependencyObject o, object basevalue)
        {
            var newValue = basevalue as LinkGroup;
            var modernMenu = (ModernMenu)o;
            if (newValue == null)
            {
                return modernMenu.LinkGroups.FirstOrDefault(x => x.Source == null);
            }

            if (modernMenu.LinkNavigator.CanNavigate(newValue.Source, modernMenu.SelectedSource, null))
            {
                modernMenu.LinkNavigator.Navigate(newValue.Source, x => {}, null);
                return modernMenu.SelectedLinkGroup;
            }
            return basevalue;
        }

        private static void OnSelectedSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!Equals(e.OldValue, e.NewValue))
            {
                ((ModernMenu)o).OnSelectedSourceChanged((Uri)e.OldValue, (Uri)e.NewValue);
            }
        }
    }
}
