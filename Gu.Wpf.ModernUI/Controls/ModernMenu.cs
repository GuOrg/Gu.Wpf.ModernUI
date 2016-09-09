namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Navigation;

    /// <summary>
    /// Represents the menu in a Modern UI styled window.
    /// </summary>
    public class ModernMenu : ItemsControl, INavigator, IList
    {
        /// <summary>
        /// Defines the SelectedLinkGroup dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedLinkGroupProperty = DependencyProperty.Register(
            "SelectedLinkGroup",
            typeof(LinkGroup),
            typeof(ModernMenu),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedLinkProperty = ModernLinks.SelectedLinkProperty.AddOwner(typeof(ModernMenu));
       
        public static readonly DependencyProperty SelectedSourceProperty = ModernLinks.SelectedSourceProperty.AddOwner(
            typeof(ModernMenu),
            new FrameworkPropertyMetadata(
                default(Uri), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(
            typeof (ModernMenu),
            new FrameworkPropertyMetadata(
                null, 
                FrameworkPropertyMetadataOptions.Inherits));
       
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
            AddHandler(CommandManager.CanExecuteEvent, new CanExecuteRoutedEventHandler((o, e) => LinkCommands.OnCanNavigateLink(this, o as UIElement, e)), true);
            var commandBinding = LinkCommands.CreateNavigateLinkCommandBinding(this);
            this.CommandBindings.Add(commandBinding);
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
        /// Explicit implementation here to only expose set to consumers of INavigator
        /// </summary>
        ILink INavigator.SelectedLink
        {
            get { return this.SelectedLinkGroup; }
            set { this.SelectedLinkGroup = (LinkGroup)value; }
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
        /// Explicit implementation here to set current value
        /// </summary>
        Uri INavigator.SelectedSource
        {
            get { return this.SelectedSource; }
            set { SetCurrentValue(SelectedSourceProperty, value); }
        }

        /// <summary>
        /// Get or sets the target frame
        /// </summary>
        public ModernFrame NavigationTarget
        {
            get { return (ModernFrame)GetValue(NavigationTargetProperty); }
            set { SetValue(NavigationTargetProperty, value); }
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
            get { return (ILinkNavigator)GetValue(LinkNavigatorProperty); }
            set { SetValue(LinkNavigatorProperty, value); }
        }

        protected override void AddChild(object value)
        {
            this.Items.Add(value);
        }

        #region IList Implementing for convenience from xaml

        public int Add(object value)
        {
            return this.Items.Add(value);
        }

        public void Clear()
        {
            this.Items.Clear();
        }

        bool IList.Contains(object value)
        {
            return this.Items.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return this.Items.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            this.Items.Insert(index, value);
        }

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;

        void IList.Remove(object value)
        {
            this.Items.Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            this.Items.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get { return this.Items[index]; }
            set { this.Items[index] = value; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.Items.CopyTo(array, index);
        }

        int ICollection.Count => this.Items.Count;

        bool ICollection.IsSynchronized => ((ICollection)this.Items).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)this.Items).SyncRoot;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Items).GetEnumerator();
        }

        #endregion IList
    }
}
