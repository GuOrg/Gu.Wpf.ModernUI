namespace Gu.Wpf.ModernUI
{
    using System;

    public interface INavigationSource : IDisposable
    {
        /// <summary>
        /// Gets or sets the collection of links that define the available content in this tab.
        /// </summary>
        LinkCollection Links { get; }
       
        /// <summary>
        /// Gets or sets the source URI of the selected link.
        /// </summary>
        /// <value>The source URI of the selected link.</value>
        Link SelectedLink { get; set; }
        
        /// <summary>
        /// Get or sets the Uri of the selected Link
        /// </summary>
        Uri SelectedSource { get; set; }

        ModernFrame NavigationTarget { get; set; }
    }
}