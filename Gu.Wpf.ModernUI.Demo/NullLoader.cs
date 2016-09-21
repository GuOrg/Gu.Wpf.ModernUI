namespace Gu.Wpf.ModernUI.Demo
{
    using System;

    public class NullLoader : DefaultContentLoader
    {
        public static readonly NullLoader Default = new NullLoader();

        protected override object LoadContent(Uri uri)
        {
            return null;
        }
    }
}