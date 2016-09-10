namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ContentLoaderImages.xaml
    /// </summary>
    public partial class ContentLoaderImages : UserControl
    {
        public ContentLoaderImages()
        {
            this.InitializeComponent();

            this.LoadImageLinks();
        }

        private async void LoadImageLinks()
        {
            //var loader = this.Tab.GetContentLoader() as FlickrImageLoader;
            //if (loader == null)
            //{
            //    return;
            //}

            //try {
            //    // load image links and assign to tab list
            //    this.Tab.Items = await loader.GetInterestingnessListAsync();

            //    // select first link
            //    this.Tab.SelectedLink = this.Tab.Items.OfType<Link>().FirstOrDefault();
            //}
            //catch (Exception e) {
            //    ModernDialog.ShowMessage(e.Message, "Failure", MessageBoxButton.OK);
            //}
        }
    }
}
