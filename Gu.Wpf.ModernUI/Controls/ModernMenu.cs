namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Gu.Wpf.ModernUI.Navigation;

    /// <summary>
    /// Represents the menu in a Modern UI styled window.
    /// </summary>
    public class ModernMenu : ItemsControl, INavigator, IList
    {
        /// <summary>Identifies the <see cref="SelectedLinkGroup"/> dependency property.</summary>
        public static readonly DependencyProperty SelectedLinkGroupProperty = DependencyProperty.Register(
            nameof(SelectedLinkGroup),
            typeof(LinkGroup),
            typeof(ModernMenu),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedLinkProperty = ModernLinks.SelectedLinkProperty.AddOwner(typeof(ModernMenu));

        /// <summary>Identifies the <see cref="SelectedSource"/> dependency property.</summary>
        public static readonly DependencyProperty SelectedSourceProperty = ModernLinks.SelectedSourceProperty.AddOwner(
            typeof(ModernMenu),
            new FrameworkPropertyMetadata(
                default(Uri),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>Identifies the <see cref="LinkNavigator"/> dependency property.</summary>
        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(
            typeof(ModernMenu),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the <see cref="NavigationTarget"/> dependency property.</summary>
        public static readonly DependencyProperty NavigationTargetProperty = Modern.NavigationTargetProperty.AddOwner(
            typeof(ModernMenu),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        static ModernMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernMenu), new FrameworkPropertyMetadata(typeof(ModernMenu)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernMenu"/> class.
        /// </summary>
        public ModernMenu()
        {
            this.AddHandler(CommandManager.CanExecuteEvent, new CanExecuteRoutedEventHandler((o, e) => LinkCommands.OnCanNavigateLink(this, o as UIElement, e)), handledEventsToo: true);
            var commandBinding = LinkCommands.CreateNavigateLinkCommandBinding(this);
            this.CommandBindings.Add(commandBinding);
        }

        /// <summary>
        /// Gets the selected link groups.
        /// </summary>
        public LinkGroup SelectedLinkGroup
        {
            get => (LinkGroup)this.GetValue(SelectedLinkGroupProperty);
            set => this.SetValue(SelectedLinkGroupProperty, value);
        }

        /// <summary>
        /// Explicit implementation here to only expose set to consumers of INavigator
        /// </summary>
        ILink INavigator.SelectedLink
        {
            get => this.SelectedLinkGroup;
            set => this.SelectedLinkGroup = (LinkGroup)value;
        }

        /// <summary>
        /// Get or sets the Uri of the selected Link
        /// </summary>
        public Uri SelectedSource
        {
            get => (Uri)this.GetValue(SelectedSourceProperty);
            set => this.SetValue(SelectedSourceProperty, value);
        }

        /// <summary>
        /// Explicit implementation here to set current value
        /// </summary>
        Uri INavigator.SelectedSource
        {
            get => this.SelectedSource;
            set => this.SetCurrentValue(SelectedSourceProperty, value);
        }

        /// <summary>
        /// Get or sets the target frame
        /// </summary>
        public ModernFrame NavigationTarget
        {
            get => (ModernFrame)this.GetValue(NavigationTargetProperty);
            set => this.SetValue(NavigationTargetProperty, value);
        }

        /// <summary>
        /// Gets a collection with all nested sublinks
        /// </summary>
        public IEnumerable<ILink> Links => this.Items.OfType<LinkGroup>();

        /// <summary>
        ///
        /// </summary>
        public ILinkNavigator LinkNavigator
        {
            get => (ILinkNavigator)this.GetValue(LinkNavigatorProperty);
            set => this.SetValue(LinkNavigatorProperty, value);
        }

        protected override void AddChild(object value)
        {
            this.Items.Add(value);
        }

#pragma warning disable SA1124, SA1201 // We use a region for the IList bloat
        #region IList Implementing for convenience from xaml

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;

        int ICollection.Count => this.Items.Count;

        bool ICollection.IsSynchronized => ((ICollection)this.Items).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)this.Items).SyncRoot;

        object IList.this[int index]
        {
            get => this.Items[index];
            set => this.Items[index] = value;
        }

        public int Add(object value) => this.Items.Add(value);

        public void Clear() => this.Items.Clear();

        bool IList.Contains(object value) => this.Items.Contains(value);

        int IList.IndexOf(object value) => this.Items.IndexOf(value);

        void IList.Insert(int index, object value) => this.Items.Insert(index, value);

        void IList.Remove(object value) => this.Items.Remove(value);

        void IList.RemoveAt(int index) => this.Items.RemoveAt(index);

        void ICollection.CopyTo(Array array, int index) => this.Items.CopyTo(array, index);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.Items).GetEnumerator();

        #endregion IList
#pragma warning restore SA1124, SA1201 // We use a region for the IList bloat
    }
}
