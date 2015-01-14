namespace Gu.ModernUI.Demo.Content
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

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
            var loader = (FlickrImageLoader)Tab.ContentLoader;

            try {
                // load image links and assign to tab list
                this.Tab.Links = await loader.GetInterestingnessListAsync();

                // select first link
                this.Tab.SelectedSource = this.Tab.Links.Select(l => l.Source).FirstOrDefault();
            }
            catch (Exception e) {
                ModernDialog.ShowMessage(e.Message, "Failure", MessageBoxButton.OK);
            }
        }
    }
}
