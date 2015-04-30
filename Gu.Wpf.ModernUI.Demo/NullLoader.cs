namespace Gu.Wpf.ModernUI.Demo
{
    using System;

    public class NullLoader : DefaultContentLoader
    {
        protected override object LoadContent(Uri uri)
        {
            return null;
        }
    }
}