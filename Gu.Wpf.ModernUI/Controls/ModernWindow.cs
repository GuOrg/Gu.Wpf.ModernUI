namespace Gu.Wpf.ModernUI
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using ModernUi.Interfaces;
    using Navigation;

    /// <summary>
    /// Represents a Modern UI styled window.
    /// </summary>
    [TemplatePart(Name = PART_WindowBorder, Type = typeof(Border))]
    [TemplatePart(Name = PART_AdornerLayer, Type = typeof(AdornerDecorator))]
    public class ModernWindow : DpiAwareWindow
    {
        private const string PART_WindowBorder = "PART_WindowBorder";
        private const string PART_AdornerLayer = "PART_AdornerLayer";
        /// <summary>
        /// Identifies the BackgroundContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BackgroundContentProperty = DependencyProperty.Register(
            "BackgroundContent", 
            typeof(object), 
            typeof(ModernWindow));
        /// <summary>
        /// Identifies the MenuLinkGroups dependency property.
        /// </summary>
        public static readonly DependencyProperty MenuLinkGroupsProperty = DependencyProperty.Register(
            "MenuLinkGroups", 
            typeof(LinkGroupCollection), 
            typeof(ModernWindow));
        /// <summary>
        /// Identifies the TitleLinks dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleLinksProperty = DependencyProperty.Register(
            "TitleLinks", 
            typeof(LinkCollection), 
            typeof(ModernWindow));

        public static readonly DependencyProperty HomeProperty = DependencyProperty.Register(
            "Home", 
            typeof(Uri), 
            typeof(ModernWindow),
            new PropertyMetadata(default(Uri)));

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
            typeof(ModernWindow));
        /// <summary>
        /// Identifies the ContentLoader dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentLoaderProperty = Modern.ContentLoaderProperty.AddOwner(typeof(ModernWindow));
        /// <summary>
        /// Identifies the LinkNavigator dependency property.
        /// </summary>
        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(typeof(ModernWindow));
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
            SetCurrentValue(MenuLinkGroupsProperty, new LinkGroupCollection());
            SetCurrentValue(TitleLinksProperty, new LinkCollection());

            // associate window commands with this instance
            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
            // associate navigate link command with this instance
            this.CommandBindings.Add(new CommandBinding(LinkCommands.NavigateLink, OnNavigateLink, OnCanNavigateLink));

            // listen for theme changes
            AppearanceManager.Current.PropertyChanged += OnAppearanceManagerPropertyChanged;
        }

        public AdornerDecorator AdornerDecorator { get; private set; }


        /// <summary>
        /// Gets or sets the background content of this window instance.
        /// </summary>
        public object BackgroundContent
        {
            get { return GetValue(BackgroundContentProperty); }
            set { SetValue(BackgroundContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of link groups shown in the window's menu.
        /// </summary>
        public LinkGroupCollection MenuLinkGroups
        {
            get { return (LinkGroupCollection)GetValue(MenuLinkGroupsProperty); }
            set { SetValue(MenuLinkGroupsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of links that appear in the menu in the title area of the window.
        /// </summary>
        public LinkCollection TitleLinks
        {
            get { return (LinkCollection)GetValue(TitleLinksProperty); }
            set { SetValue(TitleLinksProperty, value); }
        }

        /// <summary>
        /// Gets or sets the link to the home view
        /// </summary>
        public Uri Home
        {
            get { return (Uri)GetValue(HomeProperty); }
            set { SetValue(HomeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the path data for the logo displayed in the title area of the window.
        /// </summary>
        public object Logo
        {
            get { return GetValue(LogoProperty); }
            set { SetValue(LogoProperty, value); }
        }

        /// <summary>
        /// Gets or sets the source uri of the current content.
        /// </summary>
        public Uri ContentSource
        {
            get { return (Uri)GetValue(ContentSourceProperty); }
            set { SetValue(ContentSourceProperty, value); }
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
        /// The dialoghandler is used for displaying dialogs in banner style
        /// </summary>
        public IDialogHandler DialogHandler
        {
            get { return (IDialogHandler)GetValue(DialogHandlerProperty); }
            set { SetValue(DialogHandlerProperty, value); }
        }

        /// <summary>
        /// Raises the System.Windows.Window.Closed event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // detach event handler
            AppearanceManager.Current.PropertyChanged -= OnAppearanceManagerPropertyChanged;
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // retrieve BackgroundAnimation storyboard
            var border = GetTemplateChild(PART_WindowBorder) as Border;
            if (border != null)
            {
                this.backgroundAnimation = border.Resources["BackgroundAnimation"] as Storyboard;

                if (this.backgroundAnimation != null)
                {
                    this.backgroundAnimation.Begin();
                }
            }
            this.AdornerDecorator = GetTemplateChild(PART_AdornerLayer) as AdornerDecorator;
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // start background animation if theme has changed
            if (e.PropertyName == "ThemeSource" && this.backgroundAnimation != null)
            {
                this.backgroundAnimation.Begin();
            }
        }

        private void OnCanNavigateLink(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            if (this.LinkNavigator == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = this.LinkNavigator.CanNavigate(e.OriginalSource as ModernFrame, e.Parameter as Uri);
        }

        private void OnNavigateLink(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.LinkNavigator == null)
            {
                return;
            }
            e.Handled = true;
            this.LinkNavigator.Navigate(e.OriginalSource as ModernFrame, e.Parameter as Uri);
        }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
#if NET4
            Microsoft.Windows.Shell.SystemCommands.CloseWindow(this);
#else
            SystemCommands.CloseWindow(this);
#endif
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
#if NET4
            Microsoft.Windows.Shell.SystemCommands.MaximizeWindow(this);
#else
            SystemCommands.MaximizeWindow(this);
#endif
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
#if NET4
            Microsoft.Windows.Shell.SystemCommands.MinimizeWindow(this);
#else
            SystemCommands.MinimizeWindow(this);
#endif
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
#if NET4
            Microsoft.Windows.Shell.SystemCommands.RestoreWindow(this);
#else
            SystemCommands.RestoreWindow(this);
#endif
        }
    }
}
