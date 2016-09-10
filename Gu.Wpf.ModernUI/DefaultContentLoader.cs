namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    using JetBrains.Annotations;

    /// <summary>
    /// Loads XAML files using Application.LoadComponent.
    /// </summary>
    public class DefaultContentLoader : IContentLoader, INotifyPropertyChanged
    {
        public static readonly DefaultContentLoader Instance = new DefaultContentLoader(); // Not a singleton with private ctor 
        private readonly ConcurrentDictionary<Uri, TimeSpan> loadTimes = new ConcurrentDictionary<Uri, TimeSpan>(); // using concurrent for nicer api
        private readonly ConcurrentDictionary<Uri, Exception> exceptions = new ConcurrentDictionary<Uri, Exception>(); // using concurrent for nicer api
        private readonly ContentCache cache = new ContentCache();
        private bool isCaching = true;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Asynchronously loads content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri.</param>
        /// <param name="cancellationToken">The token used to cancel the load content task.</param>
        /// <returns>The loaded content.</returns>
        public virtual async Task<object> LoadContentAsync(Uri uri, CancellationToken cancellationToken)
        {
            // Not sure this async version does anything at all. Keeping it for compat.
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher == null)
            {
                throw new InvalidOperationException("Trying to load content when dispatcher == null");
            }

            object cached;
            if (this.IsCaching && this.cache.TryGetValue(uri, out cached))
            {
                return cached;
            }

            try
            {
                var sw = Stopwatch.StartNew();
                var content = await dispatcher.InvokeAsync(() => this.LoadContent(uri), DispatcherPriority.Render, cancellationToken).Task;
                sw.Stop();
                this.loadTimes.AddOrUpdate(uri, sw.Elapsed, (_, __) => sw.Elapsed);
                this.OnPropertyChanged(nameof(this.LoadTimes));
                if (this.IsCaching)
                {
                    this.cache.AddOrUpdate(uri, content);
                }

                return content;
            }
            catch (Exception e)
            {
                this.exceptions.AddOrUpdate(uri, e, (_, __) => e);
                this.OnPropertyChanged(nameof(this.Exceptions));
                throw;
            }
        }

        /// <summary>
        /// Loads the content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri</param>
        /// <returns>The loaded content.</returns>
        protected virtual object LoadContent(Uri uri)
        {
            // don't do anything in design mode
            if (Is.InDesignMode)
            {
                return null;
            }

            return Application.LoadComponent(uri);
        }

        public bool IsCaching
        {
            get { return this.isCaching; }
            set { this.isCaching = value; }
        }

        public IEnumerable<KeyValuePair<Uri, TimeSpan>> LoadTimes => this.loadTimes;

        public IEnumerable<KeyValuePair<Uri, Exception>> Exceptions => this.exceptions;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
