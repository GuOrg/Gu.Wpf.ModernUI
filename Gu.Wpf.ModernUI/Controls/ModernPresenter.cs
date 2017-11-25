namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// ModernPresenter allows controls to have multiple parents.
    /// This is useful for performance as it allows aggressive caching
    /// </summary>
    public class ModernPresenter : FrameworkElement
    {
        /// <summary>Identifies the <see cref="ContentLoader"/> dependency property.</summary>
        public static readonly DependencyProperty ContentLoaderProperty = Modern.ContentLoaderProperty.AddOwner(
                typeof(ModernPresenter),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.Inherits,
                    OnContentLoaderChanged));

        /// <summary>Identifies the <see cref="CurrentSource"/> dependency property.</summary>
        public static readonly DependencyProperty CurrentSourceProperty = DependencyProperty.Register(
            nameof(CurrentSource),
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
            this.IsVisibleChanged += this.OnIsVisibleChanged;
        }

        /// <summary>
        /// Gets or sets the content loader.
        /// </summary>
        public IContentLoader ContentLoader
        {
            get => (IContentLoader)this.GetValue(ContentLoaderProperty);
            set => this.SetValue(ContentLoaderProperty, value);
        }

        /// <summary>
        /// Gets or sets the source of the current content.
        /// </summary>
        public Uri CurrentSource
        {
            get => (Uri)this.GetValue(CurrentSourceProperty);
            set => this.SetValue(CurrentSourceProperty, value);
        }

        /// <summary>
        /// The content loaded when <see cref="ContentLoader"/> loads <see cref="CurrentSource"/>
        /// </summary>
        public UIElement Content
        {
            get => this.Child;

            protected set
            {
                var parent = GetPresenterParent(value);
                if (parent != null)
                {
                    parent.Content = null;
                }

                this.Child = value;
            }
        }

        /// <summary>
        /// The visual and logical child.
        /// </summary>
        protected virtual UIElement Child
        {
            get => this.child;

            set
            {
                if (!ReferenceEquals(this.child, value))
                {
                    // notify the visual layer that the old child has been removed.
                    this.RemoveVisualChild(this.child);

                    // need to remove old element from logical tree
                    this.RemoveLogicalChild(this.child);

                    this.child = value;

                    this.AddLogicalChild(value);

                    // notify the visual layer about the new child.
                    this.AddVisualChild(value);

                    this.InvalidateMeasure();
                }
            }
        }

        /// <inheritdoc/>
        protected override int VisualChildrenCount => (this.child == null) ? 0 : 1;

        /// <inheritdoc/>
        protected override Visual GetVisualChild(int index)
        {
            if ((this.child == null) || (index != 0))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.child;
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size constraint)
        {
            if (this.child != null)
            {
                this.child.Measure(constraint);
                return this.child.DesiredSize;
            }

            return new Size(0, 0);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            this.Child?.Arrange(new Rect(arrangeSize));
            return arrangeSize;
        }

        /// <summary>
        /// Trigger <see cref="ContentLoader"/> to load <see cref="CurrentSource"/> again
        /// </summary>
        protected virtual async void RefreshContent()
        {
            try
            {
                this.isLoading = true;
                this.Content = (UIElement)await this.ContentLoader.LoadContentAsync(this.CurrentSource, CancellationToken.None);
            }
            catch (Exception e)
            {
                this.Content = new ContentPresenter { Content = e };
            }
            finally
            {
                this.isLoading = false;
            }
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
                throw new ArgumentException($"Only ModernPresenters can share children. Other parent was {parent.GetType().Name}");
            }

            return presenter;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
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
                this.RefreshContent();
            }
        }
    }
}
