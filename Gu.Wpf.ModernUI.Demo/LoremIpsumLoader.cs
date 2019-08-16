namespace Gu.Wpf.ModernUI.Demo
{
    using System;

    using Gu.Wpf.ModernUI.Demo.Content;
    using Gu.Wpf.ModernUI;

    /// <summary>
    /// Loads lorem ipsum content regardless the given uri.
    /// </summary>
    public class LoremIpsumLoader
        : DefaultContentLoader
    {
        public static readonly IContentLoader Default = new LoremIpsumLoader();

        /// <summary>
        /// Loads the content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri</param>
        /// <returns>The loaded content.</returns>
        protected override object LoadContent(Uri uri)
        {
            // return a new LoremIpsum user control instance no matter the uri
            return new LoremIpsum(uri.ToString());
        }
    }
}
