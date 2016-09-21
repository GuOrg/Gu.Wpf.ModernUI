namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    using Gu.ModernUI.Interfaces;

    using Navigation;

    /// <summary>
    /// Represents a Modern UI styled window.
    /// </summary>
    [TemplatePart(Name = PartWindowBorder, Type = typeof(Border))]
    [TemplatePart(Name = PartAdornerLayer, Type = typeof(AdornerDecorator))]
    [TemplatePart(Name = PartContentFrame, Type = typeof(ModernFrame))]
    public class ModernWindow : DpiAwareWindow, INavigator
    {
        public const string PartWindowBorder = "PART_WindowBorder";
        public const string PartAdornerLayer = "PART_AdornerLayer";
        public const string PartContentFrame = "PART_ContentFrame";

        /// <summary>
        /// Identifies the BackgroundContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BackgroundContentProperty = DependencyProperty.Register(
            "BackgroundContent",
            typeof(object),
            typeof(ModernWindow));

        /// <summary>
        /// Identifies the MainMenu dependency property.
        /// </summary>
        public static readonly DependencyProperty MainMenuProperty = DependencyProperty.Register(
            "MainMenu",
            typeof(ModernMenu),
            typeof(ModernWindow));

        /// <summary>
        /// Identifies the TitleLinks dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleLinksProperty = DependencyProperty.Register(
            "TitleLinks",
            typeof(TitleLinks),
            typeof(ModernWindow));

        public static readonly DependencyProperty HomeProperty = DependencyProperty.Register(
            "Home",
            typeof(Link),
            typeof(ModernWindow),
            new PropertyMetadata(default(Link)));

        /// <summary>
        /// Identifies the LogoData dependency property.
        /// </summary>
        public static readonly DependencyProperty LogoProperty = DependencyProperty.Register(
            "Logo",
            typeof(object),
            typeof(ModernWindow));

        /// <summary>
        /// Defines the ContentSource dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentSourceProperty = DependencyProperty.Register(
            "ContentSource",
            typeof(Uri),
            typeof(ModernWindow),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Identifies the ContentLoader dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentLoaderProperty = Modern.ContentLoaderProperty.AddOwner(
            typeof(ModernWindow),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Identifies the LinkNavigator dependency property.
        /// </summary>
        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(
            typeof(ModernWindow),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty NavigationTargetProperty = Modern.NavigationTargetProperty.AddOwner(
            typeof(ModernWindow),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Identifies the DialogHandler dependency property.
        /// </summary>
        public static readonly DependencyProperty DialogHandlerProperty = DependencyProperty.Register(
            "DialogHandler",
            typeof(IDialogHandler),
            typeof(ModernWindow),
            new PropertyMetadata(null));

        private Storyboard backgroundAnimation;

        static ModernWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernWindow), new FrameworkPropertyMetadata(typeof(ModernWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernWindow"/> class.
        /// </summary>
        public ModernWindow()
        {
            // create empty collections
            this.SetValue(MainMenuProperty, new ModernMenu());
            this.SetValue(TitleLinksProperty, new TitleLinks());

            // associate window commands with this instance
            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));

            // associate navigate link command with this instance
            var commandBinding = LinkCommands.CreateNavigateLinkCommandBinding(this);
            this.CommandBindings.Add(commandBinding);

            // listen for theme changes
            AppearanceManager.Current.PropertyChanged += this.OnAppearanceManagerPropertyChanged;
        }

        public AdornerDecorator AdornerDecorator { get; private set; }

        /// <summary>
        /// Gets or sets the background content of this window instance.
        /// </summary>
        public object BackgroundContent
        {
            get { return this.GetValue(BackgroundContentProperty); }
            set { this.SetValue(BackgroundContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of link groups shown in the window's menu.
        /// </summary>
        public ModernMenu MainMenu
        {
            get { return (ModernMenu)this.GetValue(MainMenuProperty); }
            set { this.SetValue(MainMenuProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of links that appear in the menu in the title area of the window.
        /// </summary>
        public TitleLinks TitleLinks
        {
            get { return (TitleLinks)this.GetValue(TitleLinksProperty); }
            set { this.SetValue(TitleLinksProperty, value); }
        }

        /// <summary>
        /// Gets or sets the link to the home view
        /// </summary>
        public Link Home
        {
            get { return (Link)this.GetValue(HomeProperty); }
            set { this.SetValue(HomeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the path data for the logo displayed in the title area of the window.
        /// </summary>
        public object Logo
        {
            get { return this.GetValue(LogoProperty); }
            set { this.SetValue(LogoProperty, value); }
        }

        /// <summary>
        /// Gets or sets the source uri of the current content.
        /// </summary>
        public Uri ContentSource
        {
            get { return (Uri)this.GetValue(ContentSourceProperty); }
            set { this.SetValue(ContentSourceProperty, value); }
        }

        /// <summary>
        /// The dialoghandler is used for displaying dialogs in banner style
        /// </summary>
        public IDialogHandler DialogHandler
        {
            get { return (IDialogHandler)this.GetValue(DialogHandlerProperty); }
            set { this.SetValue(DialogHandlerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content loader.
        /// </summary>
        public IContentLoader ContentLoader
        {
            get { return (IContentLoader)this.GetValue(ContentLoaderProperty); }
            set { this.SetValue(ContentLoaderProperty, value); }
        }

#pragma warning disable CA1033

        /// <summary>
        /// Gets or sets the link navigator.
        /// </summary>
        /// <value>The link navigator.</value>
        public ILinkNavigator LinkNavigator
        {
            get { return (ILinkNavigator)this.GetValue(LinkNavigatorProperty); }
            set { this.SetValue(LinkNavigatorProperty, value); }
        }

        ILink INavigator.SelectedLink
        {
            get { return null; }
            set { /*nop*/ }
        }

        Uri INavigator.SelectedSource
        {
            get { return this.ContentSource; }
            set { this.ContentSource = value; }
        }

        IEnumerable<ILink> INavigator.Links => Enumerable.Empty<ILink>();

#pragma warning disable CA1033

        public ModernFrame NavigationTarget
        {
            get { return (ModernFrame)this.GetValue(NavigationTargetProperty); }
            set { this.SetValue(NavigationTargetProperty, value); }
        }

        /// <summary>
        /// Raises the System.Windows.Window.Closed event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // detach event handler
            AppearanceManager.Current.PropertyChanged -= this.OnAppearanceManagerPropertyChanged;
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // retrieve BackgroundAnimation storyboard
            var border = this.GetTemplateChild(PartWindowBorder) as Border;
            if (border != null)
            {
                this.backgroundAnimation = border.Resources["BackgroundAnimation"] as Storyboard;

                this.backgroundAnimation?.Begin();
            }

            this.AdornerDecorator = this.GetTemplateChild(PartAdornerLayer) as AdornerDecorator;
            this.NavigationTarget = this.GetTemplateChild(PartContentFrame) as ModernFrame;
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // start background animation if theme has changed
            if (e.PropertyName == "ThemeSource")
            {
                this.backgroundAnimation?.Begin();
            }
        }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            SystemCommands.RestoreWindow(this);
        }
    }
}
