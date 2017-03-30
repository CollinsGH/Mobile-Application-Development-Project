using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Mobile_App_Development_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            getPhotos();
        }

        // Get a list of all JPEG photos in the pictures library
        // Adapted from https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-managing-folders-in-the-music-pictures-and-videos-libraries#querying-the-media-libraries
        private async void getPhotos()
        {
            QueryOptions queryOption = new QueryOptions(CommonFileQuery.OrderByTitle, new string[] { ".jpg" });

            queryOption.FolderDepth = FolderDepth.Deep;

            Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

            IReadOnlyList<StorageFile> files = await KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(queryOption).GetFilesAsync();

            // Read Geotag of images
            foreach (var file in files)
            {
                // This does not yet work
                Geopoint geoPoint = await GeotagHelper.GetGeotagAsync(file);

                if (geoPoint != null) {
                    Debug.WriteLine(geoPoint.Position.Latitude);
                }
            }
        }

        private void elBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Navigate back to the MainPage
            Frame.Navigate(typeof(MainPage));
        }
    }
}
