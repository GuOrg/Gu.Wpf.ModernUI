namespace Gu.Wpf.ModernUI.Internals
{
    using System;
    using System.Windows;

    internal class NavigationTargetSourceChangedListener : IDisposable
    {
        private DependencyPropertyListener navigationTargetListener;

        private DependencyPropertyListener targetSourceListener;

        public NavigationTargetSourceChangedListener(DependencyObject source)
        {
            this.navigationTargetListener = new DependencyPropertyListener(source, Modern.NavigationTargetProperty);
            this.navigationTargetListener.Changed += OnNavigationTargetChanged;
        }

        public event EventHandler<DependencyPropertyChangedEventArgs> Changed;

        public void Dispose()
        {
            this.navigationTargetListener.Dispose();
            if (this.targetSourceListener != null)
            {
                this.targetSourceListener.Changed -= OnTargetSourceChanged;
                this.targetSourceListener.Dispose();
            }
        }

        private void OnNavigationTargetChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.targetSourceListener != null)
            {
                this.targetSourceListener.Changed -= OnTargetSourceChanged;
                this.targetSourceListener.Dispose();
            }
            var modernFrame = e.NewValue as ModernFrame;
            if (modernFrame != null)
            {
                if (modernFrame.Source != null)
                {
                    OnChanged(new DependencyPropertyChangedEventArgs(ModernFrame.SourceProperty, null, modernFrame.Source));
                }
                this.targetSourceListener = new DependencyPropertyListener(modernFrame, ModernFrame.SourceProperty);
                this.targetSourceListener.Changed += OnTargetSourceChanged;
            }
        }

        private void OnTargetSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnChanged(e);
        }

        protected virtual void OnChanged(DependencyPropertyChangedEventArgs e)
        {
            var handler = this.Changed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
