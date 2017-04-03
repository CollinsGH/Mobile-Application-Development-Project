using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.ApplicationModel;
using System.Threading.Tasks;
using Windows.System.Display;
using Windows.Graphics.Display;
using Windows.UI.Core;
using System.Diagnostics;
using Windows.Storage;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Storage.FileProperties;
using Windows.Devices.Geolocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Mobile_App_Development_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CameraPage : Page
    {
        // Camera previewing tutorial - https://docs.microsoft.com/en-us/windows/uwp/audio-video-camera/simple-camera-preview-access

        private MediaCapture _mediaCapture;
        private bool _isPreviewing; // Is the camera previewing
        private DisplayRequest _displayRequest; // Do not turn off display while previewing

        private Geolocator _geo;

        private AppSettings _settings;
        private DispatcherTimer _timer;
        private int _remainingTime;

        public CameraPage()
        {
            this.InitializeComponent();

            // Create a new dispatch timer and add an event handler for the Tick event
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;

            LoadSettings();

            this.Loaded += CameraPage_Loaded;
            Application.Current.Suspending += Application_Suspending;
        }
        
        private void LoadSettings()
        {
            try
            {
                // Load the application settings
                _settings = Storage.ReadSettingsFromIsoStorage();
            }
            catch (Exception ex)
            {
                // If an exception occurred, for example the file could not be opened
                // output the error message and create a new AppSettings object
                Debug.WriteLine(ex.Message);
                _settings = new AppSettings();
            }
        }

        private async void CameraPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Hide the timer grid
            grdTimer.Visibility = Visibility.Collapsed;

            // Setup settings controls
            SetSettingControls();

            // Initialise GeoLocator
            var access = await Geolocator.RequestAccessAsync();

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:
                    _geo = new Geolocator();
                    _geo.DesiredAccuracy = PositionAccuracy.Default;
                    break;
                default:
                    // There was a problem initialising GeoLocator
                    // Show an error to the user
                    break;
            }
        }

        private void SetSettingControls()
        {
            // Set the timer combo box selected item
            // If the timer is set to zero don't change it because it will be set to "None" by default
            if (_settings.Timer != 0) {
                ItemCollection items = cbTimer.Items;

                foreach (ComboBoxItem item in items) {
                    if (item.Content.Equals(_settings.Timer.ToString())) {
                        cbTimer.SelectedValue = item;
                        break;
                    }
                }
            }
        }

        private async void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            // Handle global application events only if this page is active
            if (Frame.CurrentSourcePageType == typeof(CameraPage))
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                await CleanupCameraAsync();
                deferral.Complete();
            }
        }
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Start preview when the user navigates to the camera page
            await StartPreviewAsync();
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop preview when the user navigates away from the camera page
            await CleanupCameraAsync();
        }

        private async Task StartPreviewAsync()
        {
            // Wrap in try catch because some devices may not have a camera and the user may deny access to the camera
            try
            {
                // Initialise capture device
                _mediaCapture = new MediaCapture();
                await _mediaCapture.InitializeAsync();

                // Connect the MediaCapture to the CaptureElement by setting the Source property
                PreviewControl.Source = _mediaCapture;
                await _mediaCapture.StartPreviewAsync();
                
                _isPreviewing = true;

                // Prevent the UI and the CaptureElement from rotating when the user changes the device orientation
                _displayRequest.RequestActive();
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            }
            catch (UnauthorizedAccessException)
            {
                // This will be thrown if the user denied access to the camera in privacy settings
                Debug.WriteLine("The app was denied access to the camera");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MediaCapture initialization failed. {0}", ex.Message);
            }
        }

        private async Task CleanupCameraAsync()
        {
            if (_mediaCapture != null)
            {
                // If previewing, stop
                if (_isPreviewing)
                {
                    await _mediaCapture.StopPreviewAsync();
                }

                // Release any resources that are in use so that the camera is available to other apps
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    PreviewControl.Source = null;

                    if (_displayRequest != null)
                    {
                        _displayRequest.RequestRelease();
                    }

                    _mediaCapture.Dispose();
                    _mediaCapture = null;
                });
            }
        }

        // Capture a photo to a file
        // Adapted from https://docs.microsoft.com/en-us/windows/uwp/audio-video-camera/basic-photo-video-and-audio-capture-with-mediacapture
        private void elCapture_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Check timer value in AppSettings object
            if (_settings.Timer > 0)
            {
                _remainingTime = _settings.Timer;

                // Show timer grid
                tblTimer.Text = _remainingTime.ToString();
                grdTimer.Visibility = Visibility.Visible;

                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Start();
            } else
            {
                // If the timer is set to zero then just take the photo
                TakePhoto();
            }
        }

        private void _timer_Tick(object sender, object e)
        {
            // Decrement timer
            _remainingTime--;

            // Update timer text
            tblTimer.Text = _remainingTime.ToString();

            if (_remainingTime <= 0)
            {
                // Stop the timer
                _timer.Stop();

                // Hide timer grid
                grdTimer.Visibility = Visibility.Collapsed;

                // Take the photo
                TakePhoto();
            }
        }

        private async void TakePhoto()
        {
            captureButtonStoryboard.Begin();
            
            String imageName = DateTime.Now.ToString("yyyyMMddHHmmss");

            var myPictures = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
            StorageFile file = await myPictures.SaveFolder.CreateFileAsync(imageName + ".jpg", CreationCollisionOption.GenerateUniqueName);

            using (var captureStream = new InMemoryRandomAccessStream())
            {
                await _mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), captureStream);

                using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var decoder = await BitmapDecoder.CreateAsync(captureStream);
                    var encoder = await BitmapEncoder.CreateForTranscodingAsync(fileStream, decoder);

                    var properties = new BitmapPropertySet {
                           { "System.Photo.Orientation", new BitmapTypedValue(PhotoOrientation.Normal, PropertyType.UInt16) }
                    };

                    await encoder.BitmapProperties.SetPropertiesAsync(properties);
                    await encoder.FlushAsync();

                    GeoTagImageFile(file);
                }
            }
        }

        // GeoTag the given file
        private async void GeoTagImageFile(IStorageFile image)
        {
            if (_geo != null) {
                await GeotagHelper.SetGeotagFromGeolocatorAsync(image, _geo);
            }
        }

        private void elBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Navigate back to the MainPage
            Frame.Navigate(typeof(MainPage));
        }

        private void elSettings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            svSettings.IsPaneOpen = true;
        }

        private void cbTimer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_settings != null) {
                string selectedItem = (cbTimer.SelectedItem as ComboBoxItem).Content.ToString();
                int timeInSeconds;

                // Parse the time from the selected item string
                if (int.TryParse(selectedItem, out timeInSeconds))
                {
                    _settings.Timer = timeInSeconds;
                } else
                {
                    // If the time cannot be parsed set the timer to zero
                    _settings.Timer = 0;
                }

                // Update settings file
                Storage.SaveSettingsInIsoStorage(_settings);
            }
        }
    }
}
