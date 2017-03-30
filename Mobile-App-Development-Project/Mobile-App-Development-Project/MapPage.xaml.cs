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
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Mobile_App_Development_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private LinkedList<Geopoint> _coordinates;
        private IReadOnlyList<StorageFile> _photos;

        public MapPage()
        {
            this.InitializeComponent();
            _coordinates = new LinkedList<Geopoint>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _photos = await Storage.GetPhotos();
            await GetCoordinates();
            AddPointsToMap();
        }

        // Adapted from https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/display-poi#add-a-mapicon
        private void AddPointsToMap()
        {
            int i = 0;

            // Create a MapIcon for each Geopoint in list
            foreach (Geopoint point in _coordinates) {
                MapIcon mapIcon = new MapIcon();
                mapIcon.Location = point;
                mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                mapIcon.ZIndex = 0;
                mapIcon.Title = "Photo " + (i + 1);
                
                // Add the MapIcon to the map
                MapControl.MapElements.Add(mapIcon);

                i++;
            }

            MapControl.MapElementClick += MapControl_MapElementClick;
        }

        // Adapted from http://stackoverflow.com/questions/34377203/how-to-create-mapicon-event-in-uwp
        private async void MapControl_MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            MapIcon clickedMapIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;

            // Get the index of the photo in the list from name which is in the format "Photo [index]"
            int index = Int32.Parse(clickedMapIcon.Title.Split(' ')[1]) - 1;

            // Show the image in the frame
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await _photos[index].OpenAsync(FileAccessMode.Read);
            bitmapImage.SetSource(stream);

            imgPhoto.Source = bitmapImage;
        }

        // Get a list of all JPEG photos in the pictures library
        // Adapted from https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-managing-folders-in-the-music-pictures-and-videos-libraries#querying-the-media-libraries
        private async Task GetCoordinates()
        {
            // Read Geotag of images
            foreach (var photo in _photos)
            {
                // Had problems getting the Geotag info from the image using the GeotagHelper class
                /*Geopoint geoPoint = await GeotagHelper.GetGeotagAsync(photo);

                if (geoPoint != null) {
                    Debug.WriteLine(geoPoint.Position.Latitude);
                }*/

                ImageProperties props = await photo.Properties.GetImagePropertiesAsync();

                // Check if image file has Geotag information
                if (props.Latitude != null && props.Longitude != null) {
                    BasicGeoposition pos = new BasicGeoposition() {
                        Latitude = (double)props.Latitude,
                        Longitude = (double)props.Longitude
                    };

                    Geopoint point = new Geopoint(pos);

                    // Add to the end of the list to maintain the order
                    _coordinates.AddLast(point);
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
