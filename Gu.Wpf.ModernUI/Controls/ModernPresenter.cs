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
    [ContentProperty("Child")]
    public class ModernPresenter : Decorator
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

        public ModernPresenter()
        {
            this.IsVisibleChanged += OnIsVisibleChanged;
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

        [DefaultValue(null)]
        public override UIElement Child
        {
            get
            {
                return base.Child;
            }
            set
            {
                var parent = GetPresenterParent(value);
                if (parent != null)
                {
                    parent.Child = null;
                }
                base.Child = value;
            }
        }

        protected virtual async void RefreshContent()
        {
            try
            {
                isLoading = true;
                this.Child = (UIElement)await this.ContentLoader.LoadContentAsync(this.CurrentSource, CancellationToken.None);

            }
            catch (Exception e)
            {
                Child = new ContentPresenter { Content = e };
            }
            finally
            {
                isLoading = false;
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
                throw new ArgumentException(string.Format("Only ModernPresenters can share children. Other parent was {0}", parent.GetType().Name));
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

            if (this.Child != null)
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
