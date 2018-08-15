namespace Gu.Wpf.ModernUI.Demo
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <summary>
    /// Provides an attached property determining the current Bing image and assigning it to an image or imagebrush.
    /// </summary>
    public static class BingImage
    {
        public static readonly DependencyProperty UseBingImageProperty =
            DependencyProperty.RegisterAttached(
                "UseBingImage",
                typeof(bool),
                typeof(BingImage),
                new PropertyMetadata(OnUseBingImageChanged));

        private static readonly HttpClient HttpClient = new HttpClient();

        private static BitmapImage cachedBingImage;

        private static async void OnUseBingImageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (bool)e.NewValue;
            var image = o as Image;
            var imageBrush = o as ImageBrush;

            if (!newValue || (image == null && imageBrush == null))
            {
                return;
            }

            if (cachedBingImage == null)
            {
                var url = await GetCurrentBingImageUrl();
                if (url != null)
                {
                    cachedBingImage = new BitmapImage(url);
                }
            }

            if (cachedBingImage != null)
            {
                if (image != null)
                {
                    image.Source = cachedBingImage;
                }
                else
                {
                    imageBrush.ImageSource = cachedBingImage;
                }
            }
        }

        private static async Task<Uri> GetCurrentBingImageUrl()
        {
            var result = await HttpClient.GetAsync("http://www.bing.com/hpimagearchive.aspx?format=xml&idx=0&n=2&mbl=1&mkt=en-ww");
            if (result.IsSuccessStatusCode)
            {
                using (var stream = await result.Content.ReadAsStreamAsync())
                {
                    var doc = XDocument.Load(stream);

                    var url = (string)doc.XPathSelectElement("/images/image/url");

                    return new Uri(
                               string.Format(CultureInfo.InvariantCulture, "http://bing.com{0}", url),
                               UriKind.Absolute);
                }
            }

            return null;
        }

        /// <summary>Helper for getting <see cref="UseBingImageProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="UseBingImageProperty"/> from.</param>
        /// <returns>UseBingImage property value.</returns>
        public static bool GetUseBingImage(DependencyObject element)
        {
            return (bool)element.GetValue(UseBingImageProperty);
        }

        /// <summary>Helper for setting <see cref="UseBingImageProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="UseBingImageProperty"/> on.</param>
        /// <param name="value">UseBingImage property value.</param>
        public static void SetUseBingImage(DependencyObject element, bool value)
        {
            element.SetValue(UseBingImageProperty, value);
        }
    }
}
