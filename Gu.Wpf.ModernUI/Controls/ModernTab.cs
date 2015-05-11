namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;

    using Gu.Wpf.ModernUI.Internals;
    using Gu.Wpf.ModernUI.Navigation;

    /// <summary>
    /// Represents a control that contains multiple pages that share the same space on screen.
    /// </summary>
    [TemplatePart(Name = PART_ContentFrame, Type = typeof(ModernFrame))]
    [ContentProperty("Links")]
    public class ModernTab : Control, IList, INavigator
    {
        public const string PART_ContentFrame = "PART_ContentFrame";
        public static readonly DependencyProperty OrientationProperty = ModernLinks.OrientationProperty.AddOwner(
            typeof(ModernTab),
            new FrameworkPropertyMetadata(
                Orientation.Horizontal,
                FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty SelectedLinkProperty = ModernLinks.SelectedLinkProperty.AddOwner(typeof(ModernTab));

        public static readonly DependencyProperty SelectedSourceProperty = ModernLinks.SelectedSourceProperty.AddOwner(
            typeof(ModernTab),
            new FrameworkPropertyMetadata(
                default(Uri),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty LinksProperty = LinkGroup.LinksPropertyKey.DependencyProperty.AddOwner(typeof(ModernTab));

        public static readonly DependencyProperty NavigationTargetProperty =
            Modern.NavigationTargetProperty.AddOwner(
                typeof(ModernTab),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(
            typeof(ModernTab),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        static ModernTab()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernTab), new FrameworkPropertyMetadata(typeof(ModernTab)));
        }

        public ModernTab()
        {
            var tabLinks = new TabLinks();
            SetValue(LinkGroup.LinksPropertyKey, tabLinks);
            AddHandler(CommandManager.CanExecuteEvent, new CanExecuteRoutedEventHandler((o, e) => LinkCommands.OnCanNavigateLink(this, o as UIElement, e)), true);

            BindingHelper.Bind(
                this, LinksProperty, ModernLinks.OrientationProperty,
                this, OrientationProperty,
                BindingMode.OneWayToSource,
                UpdateSourceTrigger.PropertyChanged); // dunno why dp inheritance does not work here
        }

        /// <summary>
        /// Gets or sets a value indicating how the tab should be rendered.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets the collection of links
        /// </summary>
        public TabLinks Links
        {
            get { return (TabLinks)GetValue(LinksProperty); }
        }

        IEnumerable<ILink> INavigator.Links
        {
            get { return this.Links != null ? this.Links.Links : Enumerable.Empty<ILink>(); }
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

        ILink INavigator.SelectedLink
        {
            get { return this.Links != null ? this.Links.SelectedLink : null; }
            set { /* Nop */ }
        }

        public ModernFrame NavigationTarget
        {
            get { return (ModernFrame)GetValue(NavigationTargetProperty); }
            set { SetValue(NavigationTargetProperty, value); }
        }

        /// <summary>
        /// Get or sets the Uri of the selected Link
        /// </summary>
        public Uri SelectedSource
        {
            get { return (Uri)GetValue(SelectedSourceProperty); }
            set { SetValue(ModernLinks.SelectedSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ILinkNavigator that manages navigation
        /// </summary>
        public ILinkNavigator LinkNavigator
        {
            get { return (ILinkNavigator)GetValue(LinkNavigatorProperty); }
            set { SetValue(LinkNavigatorProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            this.NavigationTarget = GetTemplateChild(PART_ContentFrame) as ModernFrame;
            base.OnApplyTemplate();
        }

        #region IList

        public int Add(object value)
        {
            return this.Links.Items.Add(value);
        }

        public void Clear()
        {
            this.Links.Items.Clear();
        }

        bool IList.Contains(object value)
        {
            return this.Links.Items.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return this.Links.Items.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            this.Links.Items.Insert(index, value);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
            this.Links.Items.Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            this.Links.Items.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get { return this.Links.Items[index]; }
            set { this.Links.Items[index] = value; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.Links.Items.CopyTo(array, index);
        }

        int ICollection.Count
        {
            get { return this.Links.Items.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this.Links.Items).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)this.Links.Items).SyncRoot; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Links.Items).GetEnumerator();
        }

        #endregion IList
    }
}
