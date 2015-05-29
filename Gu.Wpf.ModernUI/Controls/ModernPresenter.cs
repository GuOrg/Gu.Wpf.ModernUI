namespace Gu.Wpf.ModernUI
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;

    /// <summary>
    /// ModernPresenter allows controls to have multiple parents.
    /// This is useful for performance as it allows aggressive caching
    /// </summary>
    public class ModernPresenter : FrameworkElement
    {
        public static readonly DependencyProperty ContentLoaderProperty = Modern.ContentLoaderProperty.AddOwner(
                typeof(ModernPresenter),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.Inherits,
                    OnContentLoaderChanged));

        public static readonly DependencyProperty CurrentSourceProperty = DependencyProperty.Register(
            "CurrentSource",
            typeof(Uri),
            typeof(ModernPresenter),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnCurrentSourceChanged));

        private readonly WeakReference loaderReference = new WeakReference(null);
        private bool isLoading;
        private UIElement child;

        public ModernPresenter()
        {
            this.Loaded += OnIsLoaded;
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
        /// Gets or sets the source of the current content.
        /// </summary>
        public Uri CurrentSource
        {
            get { return (Uri)GetValue(CurrentSourceProperty); }
            set { SetValue(CurrentSourceProperty, value); }
        }

        public UIElement Content
        {
            get
            {
                return this.Child;
            }
            protected set
            {
                var parent = GetPresenterParent(value);
                if (parent != null)
                {
                    if (ReferenceEquals(parent, this))
                    {
                        return;
                    }
                    parent.Child = null;
                }
                this.Child = value;
            }
        }

        protected virtual UIElement Child
        {
            get
            {
                return this.child;
            }

            set
            {
                if (this.child != value)
                {
                    // notify the visual layer that the old child has been removed.
                    RemoveVisualChild(this.child);

                    //need to remove old element from logical tree
                    RemoveLogicalChild(this.child);

                    this.child = value;

                    AddLogicalChild(value);
                    // notify the visual layer about the new child.
                    AddVisualChild(value);

                    InvalidateMeasure();
                }
            }
        }

        protected virtual async void RefreshContent()
        {
            try
            {
                isLoading = true;
                this.Content = (UIElement)await this.ContentLoader.LoadContentAsync(this.CurrentSource, CancellationToken.None);
            }
            catch (Exception e)
            {
                this.Content = new ContentPresenter { Content = e };
            }
            finally
            {
                isLoading = false;
            }
        }

        protected override int VisualChildrenCount
        {
            get { return (this.child == null) ? 0 : 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if ((this.child == null) || (index != 0))
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return this.child;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            UIElement child = Child;
            if (child != null)
            {
                child.Measure(constraint);
                return (child.DesiredSize);
            }
            return (new Size());
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            UIElement child = Child;
            if (child != null)
            {
                child.Arrange(new Rect(arrangeSize));
            }
            return (arrangeSize);
        }

        private static void OnContentLoaderChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var presenter = (ModernPresenter)o;
            if (e.NewValue != null &&
                e.NewValue != presenter.loaderReference.Target &&
                presenter.CurrentSource != null)
            {
                presenter.loaderReference.Target = e.NewValue;
                presenter.RefreshContent();
            }
        }

        private static void OnCurrentSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var presenter = (ModernPresenter)o;
            if (e.NewValue != null &&
                presenter.ContentLoader != null)
            {
                presenter.RefreshContent();
            }
        }

        private static ModernPresenter GetPresenterParent(UIElement e)
        {
            if (e == null)
            {
                return null;
            }
            var parent = VisualTreeHelper.GetParent(e);
            if (parent == null)
            {
                return null;
            }
            var presenter = parent as ModernPresenter;
            if (presenter == null)
            {
                throw new ArgumentException(string.Format("Only ModernPresenters can share children. Other parent was {0}", parent.GetType().Name));
            }
            return presenter;
        }

        private void OnIsLoaded(object sender, RoutedEventArgs e)
        {
            if (this.ContentLoader == null)
            {
                return;
            }

            if (this.CurrentSource == null)
            {
                return;
            }

            if (this.Content != null)
            {
                return;
            }

            if (this.isLoading)
            {
                return;
            }

            if (this.IsVisible)
            {
                RefreshContent();
            }
        }
    }
}
