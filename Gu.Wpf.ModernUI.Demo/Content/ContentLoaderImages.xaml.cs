namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Gu.Wpf.ModernUI;

    /// <summary>
    /// Interaction logic for ContentLoaderImages.xaml
    /// </summary>
    public partial class ContentLoaderImages : UserControl
    {
        public ContentLoaderImages()
        {
            InitializeComponent();

            LoadImageLinks();
        }

        private async void LoadImageLinks()
        {
            var loader = Tab.GetContentLoader() as FlickrImageLoader;
            if (loader == null)
            {
                return;
            }

            try {
                // load image links and assign to tab list
                this.Tab.Links = await loader.GetInterestingnessListAsync();

                // select first link
                this.Tab.SelectedLink = this.Tab.Links.FirstOrDefault();
            }
            catch (Exception e) {
                ModernDialog.ShowMessage(e.Message, "Failure", MessageBoxButton.OK);
            }
        }
    }
}
