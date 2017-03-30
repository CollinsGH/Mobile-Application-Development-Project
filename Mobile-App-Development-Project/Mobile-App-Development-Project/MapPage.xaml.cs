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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Mobile_App_Development_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private LinkedList<Geopoint> _coordinates;

        public MapPage()
        {
            this.InitializeComponent();
            _coordinates = new LinkedList<Geopoint>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await GetCoordinates();
            AddPointsToMap();
        }

        // Adapted from https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/display-poi#add-a-mapicon
        private void AddPointsToMap()
        {
            // Create a MapIcon for each Geopoint in list
            foreach (Geopoint point in _coordinates) {
                MapIcon mapIcon = new MapIcon();
                mapIcon.Location = point;
                mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                mapIcon.ZIndex = 0;

                // Add the MapIcon to the map
                MapControl.MapElements.Add(mapIcon);
            }
        }

        // Get a list of all JPEG photos in the pictures library
        // Adapted from https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-managing-folders-in-the-music-pictures-and-videos-libraries#querying-the-media-libraries
        private async Task GetCoordinates()
        {
            IReadOnlyList<StorageFile> files = await Storage.GetPhotos();

            // Read Geotag of images
            foreach (var file in files)
            {
                // Had problems getting the Geotag info from the image using the GeotagHelper class
                /*Geopoint geoPoint = await GeotagHelper.GetGeotagAsync(file);

                if (geoPoint != null) {
                    Debug.WriteLine(geoPoint.Position.Latitude);
                }*/

                ImageProperties props = await file.Properties.GetImagePropertiesAsync();
                BasicGeoposition pos = new BasicGeoposition() {
                    Latitude = (double)props.Latitude,
                    Longitude = (double)props.Longitude
                };
                Geopoint point = new Geopoint(pos);

                _coordinates.AddLast(point);
            }
        }

        private void elBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Navigate back to the MainPage
            Frame.Navigate(typeof(MainPage));
        }
    }
}
