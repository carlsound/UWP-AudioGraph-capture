using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Devices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace W10_CS_Audio_Capture_Test_01
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //
        private DeviceInformationCollection audioCaptureDevicesCollection;
        private DeviceInformationCollection audioRenderDevicesCollection;
        //
        private AudioEngine engine;
        //
        public MainPage()
        {
            this.InitializeComponent();
        }
        //
        //
        //
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await PopulateRenderDeviceList();
            await PopulateCaptureDeviceList();
        }
        //
        //
        //
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            engine.Dispose();
        }
        //
        //
        //
        private async Task PopulateRenderDeviceList()
        {
            audioRenderDevicesListBox.Items.Clear();
            //
            audioRenderDevicesCollection = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioRenderSelector());
            //
            //audioRenderDevicesListBox.Items.Add("Select a Render Device:");
            //
            foreach (var device in audioRenderDevicesCollection)
            {
                audioRenderDevicesListBox.Items.Add(device.Name);
            }
            //
            audioRenderDevicesListBox.SelectedIndex = 0;
        }

        private async Task PopulateCaptureDeviceList()
        {
            audioCaptureDevicesListBox.Items.Clear();
            //
            audioCaptureDevicesCollection = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioCaptureSelector());
            //
            //audioCaptureDevicesListBox.Items.Add("Select a Capture Device:");
            //
            foreach (var device in audioCaptureDevicesCollection)
            {
                audioCaptureDevicesListBox.Items.Add(device.Name);
            }
            //
            audioCaptureDevicesListBox.SelectedIndex = 0;
        }

        private async void audioCaptureStart(object sender, RoutedEventArgs e)
        {
            engine = new AudioEngine(audioRenderDevicesCollection[audioRenderDevicesListBox.SelectedIndex], audioCaptureDevicesCollection[audioCaptureDevicesListBox.SelectedIndex]);
            await engine.InitializeAsync();
            engine.Start();
        }

        private void audioCaptureStop(object sender, RoutedEventArgs e)
        {
            engine.Stop();
            engine.Dispose();
        }
        //
        //
        //
        private void audioRenderDeviceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            audioCaptureToggleButton.IsChecked = false;
        }
        //
        private void audioCaptureDeviceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            audioCaptureToggleButton.IsChecked = false;
        }
        //
        //
        //
        private void enableEchoEffect(object sender, RoutedEventArgs e)
        {
            engine.EnableAudioCaptureEchoEffect();
        }
        //
        private void disableEchoEffect(object sender, RoutedEventArgs e)
        {
            engine.DisableAudioCaptureEchoEffect();
        }
        //
        //
        //
        private void enableReverbEffect(object sender, RoutedEventArgs e)
        {
            engine.EnableAudioCaptureReverbEffect();
        }
        //
        private void disableReverbEffect(object sender, RoutedEventArgs e)
        {
            engine.DisableAudioCaptureReverbEffect();
        }
        //
        //
        //
        private void enableLimiterEffect(object sender, RoutedEventArgs e)
        {
            engine.EnableAudioCaptureLimiterEffect();
        }
        //
        private void disableLimiterEffect(object sender, RoutedEventArgs e)
        {
            engine.DisableAudioCaptureLimiterEffect();
        }
        //
        //
        //
        private void enableEqEffect(object sender, RoutedEventArgs e)
        {
            engine.EnableAudioCaptureEqualizerEffect();
        }
        //
        private void disableEqEffect(object sender, RoutedEventArgs e)
        {
            engine.DisableAudioCaptureEqualizerEffect();
        }
    }
}
