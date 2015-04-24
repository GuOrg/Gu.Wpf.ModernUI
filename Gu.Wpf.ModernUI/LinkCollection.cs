namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Gu.Wpf.ModernUI;

    /// <summary>
    /// Represents an observable collection of links.
    /// </summary>
    public class LinkCollection
        : ObservableCollection<Link>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkCollection"/> class.
        /// </summary>
        public LinkCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkCollection"/> class that contains specified links.
        /// </summary>
        /// <param name="links">The links that are copied to this collection.</param>
        public LinkCollection(IEnumerable<Link> links)
        {
            if (links == null) {
                throw new ArgumentNullException("links");
            }
            foreach (var link in links) {
                Add(link);
            }
        }
    }
}
