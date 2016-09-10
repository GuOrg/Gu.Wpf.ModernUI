namespace Gu.Wpf.ModernUI
{
    using System;
    using System.IO.Packaging;

    using Gu.Wpf.ModernUI.Navigation;

    internal static class UriHelper
    {
        private static readonly Uri PackAppBaseUri = PackUriHelper.Create(new Uri("application://"));

        internal static Uri AsKey(this Uri uri)
        {
            var resolvedUri = GetResolvedUri(uri);
            // content is cached on uri without fragment
            var key = NavigationHelper.RemoveFragment(resolvedUri);
            return key;
        }

        internal static bool IsResourceUri(this Uri uri)
        {
            if (uri == null)
            {
                return false;
            }
            if (uri.IsAbsoluteUri)
            {
                if (uri.Scheme == PackUriHelper.UriSchemePack)
                {
                    return true;
                }
                return false;
            }

            return true;
        }

        private static Uri GetResolvedUri(Uri uri)
        {
            if (uri == null)
            {
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                return FixFileUri(uri);
            }

            return new Uri(PackAppBaseUri, uri);
        }

        private static Uri FixFileUri(Uri uri)
        {
            if (uri == null)
            {
                return null;
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