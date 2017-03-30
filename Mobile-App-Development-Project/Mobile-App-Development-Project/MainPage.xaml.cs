using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Mobile_App_Development_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IReadOnlyList<StorageFile> _photos;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Get all photos
            _photos = await Storage.GetPhotos();

            DisplayPhotos();
        }

        private async void DisplayPhotos()
        {
            // Display all photos in _photos variable in the grdAlbum grid

            // Calculate the number of columns based on screen width
            const int MAX_IMAGE_WIDTH = 200;
            const int MIN_COLUMNS = 3;
            int columns = MIN_COLUMNS;
            double screenWidth = grdAlbum.ActualWidth;
            int numColumns = (int)(screenWidth / MAX_IMAGE_WIDTH);

            if (numColumns > MIN_COLUMNS)
            {
                columns = numColumns;
            }

            // Calculate the number of rows by deviding the total number of photos by columns
            int rows = (int)Math.Ceiling((float)_photos.Count / columns);

            // Create the resulting number of rows and columns and add them to the grid
            for (int row = 0; row < rows; row++)
            {
                RowDefinition grdRow = new RowDefinition();
                grdAlbum.RowDefinitions.Add(grdRow);
            }

            for (int col = 0; col < columns; col++)
            {
                ColumnDefinition grdCol = new ColumnDefinition();
                grdAlbum.ColumnDefinitions.Add(grdCol);
            }

            // Add photos to grid
            int i = 0;

            foreach (StorageFile photo in _photos)
            {
                // Create image control
                BitmapImage bitmapImage = new BitmapImage();
                FileRandomAccessStream stream = (FileRandomAccessStream)await photo.OpenAsync(FileAccessMode.Read);
                bitmapImage.SetSource(stream);

                Image image = new Image();
                image.Source = bitmapImage;

                // Add to grdAlbum
                Grid.SetColumn(image, i % columns);
                Grid.SetRow(image, i / columns);
                grdAlbum.Children.Add(image);

                i++;
            }
        }

        private void btnNavCamera_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Navigate to the CameraPage
            Frame.Navigate(typeof(CameraPage));
        }

        private void btnNavMap_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Navigate to the MapPage
            Frame.Navigate(typeof(MapPage));
        }
    }
}
