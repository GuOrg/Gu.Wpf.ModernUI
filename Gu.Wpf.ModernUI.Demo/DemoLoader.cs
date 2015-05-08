namespace Gu.Wpf.ModernUI.Demo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using Content;
    using Internals;

    public class DemoLoader : DefaultContentLoader
    {
        public override Task<object> LoadContentAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri.ToString().Contains(typeof(SlowPageNoCache).Name))
            {
                IsCaching = false;
            }
            var content = base.LoadContentAsync(uri, cancellationToken);
            IsCaching = true;
            return content;
        }
    }
}
