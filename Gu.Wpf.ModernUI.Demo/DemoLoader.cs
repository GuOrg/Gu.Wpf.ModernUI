namespace Gu.Wpf.ModernUI.Demo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Gu.Wpf.ModernUI.Demo.Content;

    public class DemoLoader : DefaultContentLoader
    {
        public override Task<object> LoadContentAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri.ToString().Contains(typeof(SlowPageNoCache).Name))
            {
                this.IsCaching = false;
            }

            var content = base.LoadContentAsync(uri, cancellationToken);
            this.IsCaching = true;
            return content;
        }
    }
}
