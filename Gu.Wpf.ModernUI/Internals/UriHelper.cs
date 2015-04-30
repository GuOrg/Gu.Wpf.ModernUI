namespace Gu.Wpf.ModernUI.Internals
{
    using System;
    using System.IO.Packaging;

    using Gu.Wpf.ModernUI.Navigation;

    internal static class UriHelper
    {
        private static readonly Uri packAppBaseUri = PackUriHelper.Create(new Uri("application://"));

        internal static Uri AsKey(this Uri uri)
        {
            var resolvedUri = GetResolvedUri(uri);
            // content is cached on uri without fragment
            var key = NavigationHelper.RemoveFragment(resolvedUri);
            return key;
        }

        private static Uri GetResolvedUri(Uri uri)
        {
            if (uri != null)
            {
                if (uri.IsAbsoluteUri)
                {
                    return FixFileUri(uri);
                }
                return new Uri(packAppBaseUri, uri);
            }
            return (Uri)null;
        }

        private static Uri FixFileUri(Uri uri)
        {
            if (uri == null)
            {
                return uri;
            }

            if (!uri.IsAbsoluteUri)
            {
                return uri;
            }

            if (string.Compare(uri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return uri;
            }

            if (string.Compare(uri.OriginalString, 0, Uri.UriSchemeFile, 0, Uri.UriSchemeFile.Length, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return new Uri(uri.AbsoluteUri);
            }
            return uri;
        }
    }
}