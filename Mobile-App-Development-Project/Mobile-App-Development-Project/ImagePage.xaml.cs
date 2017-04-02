using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.BulkAccess;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Mobile_App_Development_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImagePage : Page
    {
        private StorageFile _photo;

        public ImagePage()
        {
            this.InitializeComponent();

            // Register event handlers for sharing an image
            RegisterForShare();
        }

        // Passing parameter to a new page found at http://stackoverflow.com/questions/35304615/pass-some-parameters-between-pages-in-uwp
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Get the image passed in as a parameter
            base.OnNavigatedTo(e);
            _photo = (StorageFile)e.Parameter;

            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await _photo.OpenAsync(FileAccessMode.Read);
            bitmapImage.SetSource(stream);

            // Set the image source
            imgPhoto.Source = bitmapImage;
        }

        private void elBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Navigate back to the MainPage
            Frame.Navigate(typeof(MainPage));
        }

        private void rctShare_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        // Code for sharing an image with other apps adapted from the MSDN website at
        // https://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh871370.aspx
        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.ShareImageHandler);
        }

        private void ShareImageHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;

            ResourceCandidate resource;

            resource = ResourceManager.Current.MainResourceMap.GetValue("Resources/uidShareTitle", ResourceContext.GetForCurrentView());
            request.Data.Properties.Title = resource.ValueAsString;

            resource = ResourceManager.Current.MainResourceMap.GetValue("Resources/uidShareDescription", ResourceContext.GetForCurrentView());
            request.Data.Properties.Description = resource.ValueAsString;

            // Because we are making async calls in the DataRequested event handler,
            //  we need to get the deferral first.
            DataRequestDeferral deferral = request.GetDeferral();

            // Make sure to always call Complete on the deferral.
            try
            {
                RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(_photo);
                request.Data.SetBitmap(imageStreamRef);
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
