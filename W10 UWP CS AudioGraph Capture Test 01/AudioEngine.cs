using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Audio;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Media.Render;

namespace W10_CS_Audio_Capture_Test_01
{
    class AudioEngine
    {
        private AudioGraph audGraph;
        private AudioGraphSettings audGraphSettings;
        private CreateAudioGraphResult audGraphResult;
        //private AudioEncodingProperties audEncodingProperties;
        //
        private DeviceInformation inputDevice;
        //
        private AudioDeviceInputNode deviceInputNode;
        private CreateAudioDeviceInputNodeResult deviceInputNodeResult;
        //
        private AudioDeviceOutputNode deviceOutputNode;
        private CreateAudioDeviceOutputNodeResult deviceOutputNodeResult;
        //
        private AudioSubmixNode audioDeviceOutputSubmixNode;
        //
        //private DeviceInformationCollection outputDevices;
        //
        private EchoEffectDefinition echoEffectDefinition;
        private ReverbEffectDefinition reverbEffectDefinition;
        private EqualizerEffectDefinition eqEffectDefinition;
        private LimiterEffectDefinition limiterEffectDefinition;
        //
        //
        public AudioEngine(DeviceInformation audioRenderDevice, DeviceInformation audioCaptureDevice)
        {
            inputDevice = audioCaptureDevice;
            //
            //
            //
            //audEncodingProperties = new Windows.Media.MediaProperties.AudioEncodingProperties();
            //
            //audEncodingProperties.Bitrate = 3072000;
            //audEncodingProperties.BitsPerSample = 32;
            //audEncodingProperties.ChannelCount = 2;
            //audEncodingProperties.SampleRate = 48000;
            //audEncodingProperties.Subtype = "Float";
            //
            //
            //
            audGraphSettings = new AudioGraphSettings(AudioRenderCategory.Media);
            //
            //audGraphSettings.AudioRenderCategory = AudioRenderCategory.Media;
            audGraphSettings.DesiredRenderDeviceAudioProcessing = Windows.Media.AudioProcessing.Raw;
            //audGraphSettings.DesiredSamplesPerQuantum = 480;
            //audGraphSettings.EncodingProperties = audEncodingProperties;
            audGraphSettings.QuantumSizeSelectionMode = QuantumSizeSelectionMode.LowestLatency;
            audGraphSettings.PrimaryRenderDevice = audioRenderDevice;
            //
            //
            //
            //InitializeAsync();
        }
        //
        //
        //
        public async Task InitializeAsync()
        {
            audGraphResult = await AudioGraph.CreateAsync(audGraphSettings);
            //
            audGraph = audGraphResult.Graph;
            //
            //
            //
            deviceOutputNodeResult = await audGraph.CreateDeviceOutputNodeAsync();
            //
            deviceOutputNode = deviceOutputNodeResult.DeviceOutputNode;
            //
            //
            //
            //deviceInputNodeResult = await audGraph.CreateDeviceInputNodeAsync(MediaCategory.Other);
           // deviceInputNodeResult = await audGraph.CreateDeviceInputNodeAsync(MediaCategory.Other, audGraph.EncodingProperties);
            deviceInputNodeResult = await audGraph.CreateDeviceInputNodeAsync(MediaCategory.Other, audGraph.EncodingProperties, inputDevice);
            //
            deviceInputNode = deviceInputNodeResult.DeviceInputNode;
            //
            //
            //
            audioDeviceOutputSubmixNode = audGraph.CreateSubmixNode();
            //
            //
            //
            deviceInputNode.AddOutgoingConnection(audioDeviceOutputSubmixNode);
            //
            audioDeviceOutputSubmixNode.AddOutgoingConnection(deviceOutputNode);
            //
            //
            //
            CreateEchoEffect();
            CreateReverbEffect();
            CreateLimiterEffect();
            CreateEqEffect();
        }
        //
        //
        //
        //private async Task CreateAudioGraphAsync()
        //{
        //    audGraphResult = await AudioGraph.CreateAsync(audGraphSettings);
        //    var b = 0;
        //}
        //
        //
        //
        //private async Task CreateAudioGraphDeviceOutputNodeAsync()
        //{
        //    deviceOutputNodeResult = await audGraph.CreateDeviceOutputNodeAsync();
        //}
        //
        //
        //
        //private async Task CreateAudioGraphDeviceInputNodeAsync()
        //{
        //    deviceInputNodeResult = await audGraph.CreateDeviceInputNodeAsync(MediaCategory.Other);
        //}
        //
        //
        //
        public void Dispose()
        {
            if (audGraph != null)
            {
                audGraph.Dispose();
            }
        }
        //
        //
        //
        private void CreateEchoEffect()
        {
            // create echo effect
            echoEffectDefinition = new EchoEffectDefinition(audGraph);
            //
            // See the MSDN page for parameter explanations
            // http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xapofx.fxecho_parameters(v=vs.85).aspx
            echoEffectDefinition.WetDryMix = 0.7f;
            echoEffectDefinition.Feedback = 0.5f;
            echoEffectDefinition.Delay = 500.0f;
            //
            audioDeviceOutputSubmixNode.EffectDefinitions.Add(echoEffectDefinition);
            audioDeviceOutputSubmixNode.DisableEffectsByDefinition(echoEffectDefinition);
        }
        //
        private void CreateReverbEffect()
        {
            // Create reverb effect
            reverbEffectDefinition = new ReverbEffectDefinition(audGraph);
            //
            // See the MSDN page for parameter explanations
            // https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xaudio2.xaudio2fx_reverb_parameters(v=vs.85).aspx
            reverbEffectDefinition.WetDryMix = 50;
            reverbEffectDefinition.ReflectionsDelay = 120;
            reverbEffectDefinition.ReverbDelay = 30;
            reverbEffectDefinition.RearDelay = 3;
            reverbEffectDefinition.DecayTime = 2;
            //
            audioDeviceOutputSubmixNode.EffectDefinitions.Add(reverbEffectDefinition);
            audioDeviceOutputSubmixNode.DisableEffectsByDefinition(reverbEffectDefinition);
        }
        //
        private void CreateLimiterEffect()
        {
            // Create limiter effect
            limiterEffectDefinition = new LimiterEffectDefinition(audGraph);
            //
            limiterEffectDefinition.Loudness = 1000;
            limiterEffectDefinition.Release = 10;
            //
            audioDeviceOutputSubmixNode.EffectDefinitions.Add(limiterEffectDefinition);
            audioDeviceOutputSubmixNode.DisableEffectsByDefinition(limiterEffectDefinition);
        }
        //
        private void CreateEqEffect()
        {
            // See the MSDN page for parameter explanations
            // https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xapofx.fxeq_parameters(v=vs.85).aspx
            eqEffectDefinition = new EqualizerEffectDefinition(audGraph);
            eqEffectDefinition.Bands[0].FrequencyCenter = 100.0f;
            eqEffectDefinition.Bands[0].Gain = 4.033f;
            eqEffectDefinition.Bands[0].Bandwidth = 1.5f;
            //
            eqEffectDefinition.Bands[1].FrequencyCenter = 900.0f;
            eqEffectDefinition.Bands[1].Gain = 1.6888f;
            eqEffectDefinition.Bands[1].Bandwidth = 1.5f;
            //
            eqEffectDefinition.Bands[2].FrequencyCenter = 5000.0f;
            eqEffectDefinition.Bands[2].Gain = 2.4702f;
            eqEffectDefinition.Bands[2].Bandwidth = 1.5f;
            //
            eqEffectDefinition.Bands[3].FrequencyCenter = 12000.0f;
            eqEffectDefinition.Bands[3].Gain = 5.5958f;
            eqEffectDefinition.Bands[3].Bandwidth = 2.0f;
            //
            audioDeviceOutputSubmixNode.EffectDefinitions.Add(eqEffectDefinition);
            audioDeviceOutputSubmixNode.DisableEffectsByDefinition(eqEffectDefinition);
        }
        //
        //
        //
        public void Start()
        {
            audGraph.Start();
        }
        //
        public void Stop()
        {
            audGraph.Stop();
        }
        //
        //
        //
        public void EnableAudioCaptureEchoEffect()
        {
            audioDeviceOutputSubmixNode.EnableEffectsByDefinition(echoEffectDefinition);
        }
        public void DisableAudioCaptureEchoEffect()
        {
            audioDeviceOutputSubmixNode.DisableEffectsByDefinition(echoEffectDefinition);
        }
        public void EnableAudioCaptureReverbEffect()
        {
            audioDeviceOutputSubmixNode.EnableEffectsByDefinition(reverbEffectDefinition);
        }
        public void DisableAudioCaptureReverbEffect()
        {
            audioDeviceOutputSubmixNode.DisableEffectsByDefinition(reverbEffectDefinition);
        }
        public void EnableAudioCaptureLimiterEffect()
        {
            audioDeviceOutputSubmixNode.EnableEffectsByDefinition(limiterEffectDefinition);
        }
        public void DisableAudioCaptureLimiterEffect()
        {
            audioDeviceOutputSubmixNode.DisableEffectsByDefinition(limiterEffectDefinition);
        }
        public void EnableAudioCaptureEqualizerEffect()
        {
            audioDeviceOutputSubmixNode.EnableEffectsByDefinition(eqEffectDefinition);
        }
        public void DisableAudioCaptureEqualizerEffect()
        {
            audioDeviceOutputSubmixNode.DisableEffectsByDefinition(eqEffectDefinition);
        }
    }
}
