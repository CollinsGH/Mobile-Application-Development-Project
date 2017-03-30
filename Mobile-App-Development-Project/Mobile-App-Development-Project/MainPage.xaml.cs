using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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

        private void DisplayPhotos()
        {
            // Display all photos in _photos variable in the grdAlbum grid

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
