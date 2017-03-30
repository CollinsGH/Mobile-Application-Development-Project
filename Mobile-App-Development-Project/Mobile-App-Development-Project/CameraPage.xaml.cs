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

        public CameraPage()
        {
            this.InitializeComponent();

            Application.Current.Suspending += Application_Suspending;
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
        private void elCapture_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
