namespace Gu.Wpf.ModernUI.Internals
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Helper class for managing cached content
    /// </summary>
    internal class ContentCache
    {
        private readonly Dictionary<Uri, WeakReference> cache = new Dictionary<Uri, WeakReference>();

        /// <summary>
        /// 
        /// </summary>
        public ContentCache()
        {
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
            if (uri == null)
            {
                return;
            }
            var key = uri.AsKey();
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
            var key = newValue.AsKey();
            WeakReference reff;
            if (this.cache.TryGetValue(key, out reff))
            {
                newContent = reff.Target;
            }
            return newContent != null;
        }
    }
}
