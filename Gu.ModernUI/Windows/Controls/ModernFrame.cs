namespace Gu.ModernUI.Windows.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Gu.ModernUI.Windows.Media;
    using Gu.ModernUI.Windows.Navigation;

    /// <summary>
    /// A simple content frame implementation with navigation support.
    /// </summary>
    public class ModernFrame
        : ContentControl
    {
        /// <summary>
        /// Identifies the KeepAlive attached dependency property.
        /// </summary>
        public static readonly DependencyProperty KeepAliveProperty = DependencyProperty.RegisterAttached("KeepAlive", typeof(bool?), typeof(ModernFrame), new PropertyMetadata(null));
        /// <summary>
        /// Identifies the KeepContentAlive dependency property.
        /// </summary>
        public static readonly DependencyProperty KeepContentAliveProperty = DependencyProperty.Register("KeepContentAlive", typeof(bool), typeof(ModernFrame), new PropertyMetadata(true, OnKeepContentAliveChanged));
        /// <summary>
        /// Identifies the ContentLoader dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentLoaderProperty = DependencyProperty.Register(
            "ContentLoader",
            typeof(IContentLoader),
            typeof(ModernFrame),
            new FrameworkPropertyMetadata(
                new DefaultContentLoader(),
                FrameworkPropertyMetadataOptions.Inherits,
                OnContentLoaderChanged));
        /// <summary>
        /// Identifies the LinkNavigator dependency property.
        /// </summary>
        public static DependencyProperty LinkNavigatorProperty = DependencyProperty.Register(
            "LinkNavigator",
            typeof(ILinkNavigator),
            typeof(ModernFrame),
            new FrameworkPropertyMetadata(
                new DefaultLinkNavigator(),
                FrameworkPropertyMetadataOptions.Inherits));

        private static readonly DependencyPropertyKey IsLoadingContentPropertyKey = DependencyProperty.RegisterReadOnly("IsLoadingContent", typeof(bool), typeof(ModernFrame), new PropertyMetadata(false));
        /// <summary>
        /// Identifies the IsLoadingContent dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLoadingContentProperty = IsLoadingContentPropertyKey.DependencyProperty;
        /// <summary>
        /// Identifies the Source dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(ModernFrame), new PropertyMetadata(OnSourceChanged));

        /// <summary>
        /// Occurs when navigation to a content fragment begins.
        /// </summary>
        public event EventHandler<FragmentNavigationEventArgs> FragmentNavigation;
        /// <summary>
        /// Occurs when a new navigation is requested.
        /// </summary>
        /// <remarks>
        /// The navigating event is also raised when a parent frame is navigating. This allows for cancelling parent navigation.
        /// </remarks>
        public event EventHandler<NavigatingCancelEventArgs> Navigating;
        /// <summary>
        /// Occurs when navigation to new content has completed.
        /// </summary>
        public event EventHandler<NavigationEventArgs> Navigated;
        /// <summary>
        /// Occurs when navigation has failed.
        /// </summary>
        public event EventHandler<NavigationFailedEventArgs> NavigationFailed;

        private readonly Stack<Uri> history = new Stack<Uri>();

        private readonly ContentCache contentCache;
        private readonly List<WeakReference<ModernFrame>> childFrames = new List<WeakReference<ModernFrame>>();        // list of registered frames in sub tree
        private CancellationTokenSource tokenSource;
        private bool isNavigatingHistory;
        private bool isResetSource;

        static ModernFrame()
        {
            //ContentProperty.OverrideMetadata(typeof(ModernFrame), new PropertyMetadata(OnContentChanged));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernFrame), new FrameworkPropertyMetadata(typeof(ModernFrame)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernFrame"/> class.
        /// </summary>
        public ModernFrame()
        {
            // associate application and navigation commands with this instance
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, OnBrowseBack, OnCanBrowseBack));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.GoToPage, OnGoToPage, OnCanGoToPage));
            //this.CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, OnRefresh, OnCanRefresh));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, OnCopy, OnCanCopy));
            this.contentCache = new ContentCache(this);
        }

        private static void OnKeepContentAliveChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((ModernFrame)o).OnKeepContentAliveChanged((bool)e.NewValue);
        }

        private void OnKeepContentAliveChanged(bool keepAlive)
        {
            // clear content cache
            if (!keepAlive)
            {
                this.contentCache.Clear();
            }
        }

        private static void OnContentLoaderChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                // null values for content loader not allowed
                throw new ArgumentNullException("ContentLoader");
            }
        }

        private static void OnSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!Equals(e.OldValue, e.NewValue))
            {
                ((ModernFrame)o).OnSourceChanged((Uri)e.OldValue, (Uri)e.NewValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnSourceChanged(Uri oldValue, Uri newValue)
        {
            // if resetting source or old source equals new, don't do anything
            if (this.isResetSource || newValue != null && newValue.Equals(oldValue))
            {
                return;
            }

            // handle fragment navigation
            string newFragment = null;
            var oldValueNoFragment = NavigationHelper.RemoveFragment(oldValue);
            var newValueNoFragment = NavigationHelper.RemoveFragment(newValue, out newFragment);

            if (newValueNoFragment != null && newValueNoFragment.Equals(oldValueNoFragment))
            {
                // fragment navigation
                var args = new FragmentNavigationEventArgs
                {
                    Fragment = newFragment
                };

                OnFragmentNavigation(this.Content as IContent, args);
            }
            else
            {
                var navType = this.isNavigatingHistory 
                    ? NavigationType.Back
                    : NavigationType.New;

                // only invoke CanNavigate for new navigation
                if (!this.isNavigatingHistory && !CanNavigate(oldValue, newValue, navType))
                {
                    return;
                }

                Navigate(oldValue, newValue, navType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="navigationType"></param>
        /// <returns></returns>
        protected virtual bool CanNavigate(Uri oldValue, Uri newValue, NavigationType navigationType)
        {
            var cancelArgs = new NavigatingCancelEventArgs(this, newValue, false, navigationType);

            OnNavigating(this.Content as IContent, cancelArgs);

            // check if navigation cancelled
            if (cancelArgs.Cancel)
            {
                Debug.WriteLine("Cancelled navigation from '{0}' to '{1}'", oldValue, newValue);

                if (this.Source != oldValue)
                {
                    // enqueue the operation to reset the source back to the old value
                    this.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        this.isResetSource = true;
                        SetCurrentValue(SourceProperty, oldValue);
                        this.isResetSource = false;
                    }));
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="navigationType"></param>
        protected virtual async void Navigate(Uri oldValue, Uri newValue, NavigationType navigationType)
        {
            Debug.WriteLine("Navigating from '{0}' to '{1}'", oldValue, newValue);
            // set IsLoadingContent state
            SetValue(IsLoadingContentPropertyKey, true);

            // cancel previous load content task (if any)
            // note: no need for thread synchronization, this code always executes on the UI thread
            if (this.tokenSource != null)
            {
                this.tokenSource.Cancel();
                this.tokenSource = null;
            }

            // push previous source onto the history stack (only for new navigation types)
            if (oldValue != null && navigationType == NavigationType.New)
            {
                this.history.Push(oldValue);
                this.contentCache.AddOrUpdate(oldValue, this.Content);
            }

            if (newValue == null)
            {
                SetContent(null, navigationType, null, true);
                SetValue(IsLoadingContentPropertyKey, false);
                return;
            }

            object newContent = null;

            if (navigationType == NavigationType.Refresh || !this.contentCache.TryGetValue(newValue, out newContent))
            {
                var localTokenSource = new CancellationTokenSource();
                this.tokenSource = localTokenSource;
                try
                {
                    var cancellationToken = this.tokenSource.Token;
                    cancellationToken.ThrowIfCancellationRequested();
                    newContent = await this.ContentLoader.LoadContentAsync(newValue, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine("Cancelled navigation to '{0}'", newValue);
                    SetValue(IsLoadingContentPropertyKey, false);
                    return;
                }
                catch (Exception e)
                {
                    // raise failed event
                    var failedArgs = new NavigationFailedEventArgs(this, newValue, e);

                    OnNavigationFailed(failedArgs);

                    // if not handled, show error as content
                    newContent = failedArgs.Handled ? null : failedArgs.Error;

                    SetContent(newValue, navigationType, newContent, true);
                    SetValue(IsLoadingContentPropertyKey, false);
                    return;
                }
                finally
                {
                    // clear global tokenSource to avoid a Cancel on a disposed object
                    if (this.tokenSource == localTokenSource)
                    {
                        this.tokenSource = null;
                    }

                    // and dispose of the local tokensource
                    localTokenSource.Dispose();
                }

            }

            // newValue is null or newContent was found in the cache
            SetContent(newValue, navigationType, newContent, false);
            SetValue(IsLoadingContentPropertyKey, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldParent"></param>
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            var parent = this.FindParentFrame();
            if (parent != null)
            {
                parent.RegisterChildFrame(this);
            }
        }

        private void SetContent(Uri newSource, NavigationType navigationType, object newContent, bool contentIsError)
        {
            var oldContent = this.Content;

            // assign content
            this.Content = newContent;

            // do not raise navigated event when error
            if (!contentIsError)
            {
                var args = new NavigationEventArgs(this, newSource, navigationType, newContent);
                OnNavigated(oldContent, newContent, args);
            }

            // set IsLoadingContent to false
            SetValue(IsLoadingContentPropertyKey, false);

            if (!contentIsError)
            {
                // and raise optional fragment navigation events
                string fragment;
                NavigationHelper.RemoveFragment(newSource, out fragment);
                if (fragment != null)
                {
                    // fragment navigation
                    var fragmentArgs = new FragmentNavigationEventArgs
                    {
                        Fragment = fragment
                    };

                    OnFragmentNavigation(newContent, fragmentArgs);
                }
            }
        }

        private IEnumerable<ModernFrame> GetChildFrames()
        {
            var refs = this.childFrames.ToArray();
            foreach (var r in refs)
            {
                var valid = false;
                ModernFrame frame;
                if (r.TryGetTarget(out frame))
                {
                    // check if frame is still an actual child (not the case when child is removed, but not yet garbage collected)
                    if (ReferenceEquals(frame.FindParentFrame(), this))
                    {
                        valid = true;
                        yield return frame;
                    }
                }

                if (!valid)
                {
                    this.childFrames.Remove(r);
                }
            }
        }

        private void OnFragmentNavigation(object content, FragmentNavigationEventArgs e)
        {
            var c = content as IContent;
            // invoke optional IContent.OnFragmentNavigation
            if (c != null)
            {
                c.OnFragmentNavigation(e);
            }

            // raise the FragmentNavigation event
            var handler = this.FragmentNavigation;
            if (handler != null)
            {
                handler(this, e);
            }
            NavigationEvents.OnFragmentNavigation(this, e);
        }

        private void OnNavigating(object content, NavigatingCancelEventArgs e)
        {
            // first invoke child frame navigation events
            foreach (var f in GetChildFrames())
            {
                var navigatingCancelEventArgs = new NavigatingCancelEventArgs(f, null, true, NavigationType.Parent);
                f.OnNavigating(f.Content, navigatingCancelEventArgs);
                if (navigatingCancelEventArgs.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // invoke IContent.OnNavigating (only if content implements IContent)
            var c = content as IContent;
            if (c != null)
            {
                c.OnNavigatingFrom(e);
            }

            // raise the Navigating event
            var handler = this.Navigating;
            if (handler != null)
            {
                handler(this, e);
            }
            NavigationEvents.OnNavigating(this, e);
        }

        private void OnNavigated(object oldContent, object newContent, NavigationEventArgs e)
        {
            // invoke IContent.OnNavigatedFrom and OnNavigatedTo
            if (oldContent != null)
            {
                var content = oldContent as IContent;
                if (content != null)
                {
                    content.OnNavigatedFrom(e);
                }
                // first invoke child frame navigation events
                foreach (var f in GetChildFrames())
                {
                    f.OnNavigated(f.Content, null, new NavigationEventArgs(f, null, NavigationType.Parent, null));
                }
            }
            if (newContent != null)
            {
                var content = newContent as IContent;
                if (content != null)
                {
                    content.OnNavigatedTo(e);
                }
            }

            // raise the Navigated event
            var handler = this.Navigated;
            if (handler != null)
            {
                handler(this, e);
            }
            NavigationEvents.OnNavigated(this, e);
        }

        private void OnNavigationFailed(NavigationFailedEventArgs e)
        {
            var handler = NavigationFailed;
            if (handler != null)
            {
                handler(this, e);
            }
            NavigationEvents.OnNavigationFailed(this, e);

        }

        /// <summary>
        /// Determines whether the routed event args should be handled.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <remarks>This method prevents parent frames from handling routed commands.</remarks>
        private bool HandleRoutedEvent(CanExecuteRoutedEventArgs args)
        {
            var originalSource = args.OriginalSource as DependencyObject;

            if (originalSource == null)
            {
                return false;
            }
            return originalSource.AncestorsAndSelf().OfType<ModernFrame>().FirstOrDefault() == this;
        }

        private void OnCanBrowseBack(object sender, CanExecuteRoutedEventArgs e)
        {
            // only enable browse back for source frame, do not bubble
            if (HandleRoutedEvent(e))
            {
                e.CanExecute = this.history.Count > 0;
            }
        }

        private void OnCanCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            if (HandleRoutedEvent(e))
            {
                e.CanExecute = this.Content != null;
            }
        }

        private void OnCanGoToPage(object sender, CanExecuteRoutedEventArgs e)
        {
            if (HandleRoutedEvent(e))
            {
                e.CanExecute = e.Parameter is String || e.Parameter is Uri;
            }
        }

        private void OnCanRefresh(object sender, CanExecuteRoutedEventArgs e)
        {
            if (HandleRoutedEvent(e))
            {
                e.CanExecute = this.Source != null;
            }
        }

        private void OnBrowseBack(object target, ExecutedRoutedEventArgs e)
        {
            if (this.history.Count > 0)
            {
                var oldValue = this.Source;
                var newValue = this.history.Peek();     // do not remove just yet, navigation may be cancelled

                if (CanNavigate(oldValue, newValue, NavigationType.Back))
                {
                    this.isNavigatingHistory = true;
                    SetCurrentValue(SourceProperty, this.history.Pop());
                    this.isNavigatingHistory = false;
                }
            }
        }

        private void OnGoToPage(object target, ExecutedRoutedEventArgs e)
        {
            var newValue = NavigationHelper.ToUri(e.Parameter);
            SetCurrentValue(SourceProperty, newValue);
        }

        private void OnRefresh(object target, ExecutedRoutedEventArgs e)
        {
            if (CanNavigate(this.Source, this.Source, NavigationType.Refresh))
            {
                Navigate(this.Source, this.Source, NavigationType.Refresh);
            }
        }

        private void OnCopy(object target, ExecutedRoutedEventArgs e)
        {
            // copies the string representation of the current content to the clipboard
            Clipboard.SetText(this.Content.ToString());
        }

        private void RegisterChildFrame(ModernFrame frame)
        {
            // do not register existing frame
            if (!GetChildFrames().Contains(frame))
            {
                var r = new WeakReference<ModernFrame>(frame);
                this.childFrames.Add(r);
            }
        }

        /// <summary>
        /// Determines whether the specified content should be kept alive.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool ShouldKeepContentAlive(object content)
        {
            var o = content as DependencyObject;
            if (o != null)
            {
                var result = GetKeepAlive(o);

                // if a value exists for given content, use it
                if (result.HasValue)
                {
                    return result.Value;
                }
            }
            // otherwise let the ModernFrame decide
            return this.KeepContentAlive;
        }

        /// <summary>
        /// Gets a value indicating whether to keep specified object alive in a ModernFrame instance.
        /// </summary>
        /// <param name="o">The target dependency object.</param>
        /// <returns>Whether to keep the object alive. Null to leave the decision to the ModernFrame.</returns>
        public static bool? GetKeepAlive(DependencyObject o)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            return (bool?)o.GetValue(KeepAliveProperty);
        }

        /// <summary>
        /// Sets a value indicating whether to keep specified object alive in a ModernFrame instance.
        /// </summary>
        /// <param name="o">The target dependency object.</param>
        /// <param name="value">Whether to keep the object alive. Null to leave the decision to the ModernFrame.</param>
        public static void SetKeepAlive(DependencyObject o, bool? value)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            o.SetValue(KeepAliveProperty, value);
        }

        /// <summary>
        /// Gets or sets a value whether content should be kept in memory.
        /// </summary>
        public bool KeepContentAlive
        {
            get { return (bool)GetValue(KeepContentAliveProperty); }
            set { SetValue(KeepContentAliveProperty, value); }
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
        /// Gets a value indicating whether this instance is currently loading content.
        /// </summary>
        public bool IsLoadingContent
        {
            get { return (bool)GetValue(IsLoadingContentProperty); }
        }

        /// <summary>
        /// Gets or sets the source of the current content.
        /// </summary>
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Helper class for managing cached content
        /// </summary>
        public class ContentCache
        {
            private readonly ModernFrame frame;

            private readonly Dictionary<Uri, WeakReference> cache = new Dictionary<Uri, WeakReference>();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="frame"></param>
            public ContentCache(ModernFrame frame)
            {
                this.frame = frame;
            }

            /// <summary>
            /// Clear
            /// </summary>
            public void Clear()
            {
                this.cache.Clear();
            }

            /// <summary>
            /// Adds or updates 
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="content"></param>
            public void AddOrUpdate(Uri uri, object content)
            {
                if (uri == null || !this.frame.KeepContentAlive)
                {
                    return;
                }
                var key = NavigationHelper.RemoveFragment(uri);
                // ConcurrentDictionary should not be needed as things will happen on UI-thread.
                if (this.cache.ContainsKey(key))
                {
                    this.cache[key].Target = content;
                }
                else
                {
                    this.cache.Add(key, new WeakReference(content));
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="newValue"></param>
            /// <param name="newContent"></param>
            /// <returns></returns>
            public bool TryGetValue(Uri newValue, out object newContent)
            {
                newContent = null;
                if (newValue == null)
                {
                    return false;
                }
                // content is cached on uri without fragment
                var key = NavigationHelper.RemoveFragment(newValue);
                WeakReference reff;
                if (this.cache.TryGetValue(key, out reff))
                {
                    newContent = reff.Target;
                }
                return newContent != null;
            }
        }
    }
}
