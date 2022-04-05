using LiveChartsCore;
using LiveChartsCore.Defaults; 
using LiveChartsCore.Kernel;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using ManagedBass;
using static DesktopClient.Helpers.SpeedTest;
using static DesktopClient.Helpers.DevicesScanner;
using DesktopClient.Helpers;
using Avalonia.Threading;
using Avalonia.Media.Imaging;

namespace DesktopClient.ViewModels
{
    public class SystemRequirmentsViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment => "/SystemRequirments";

        public IScreen HostScreen { get; }

        IEnumerable<ISeries> ChartBuilder { get; set; }
        
        private ObservableValue _internetSpeed;

        public ObservableValue InternetSpeed
        {
            get { return _internetSpeed; }
            set { _internetSpeed = value; }
        }

        private List<DeviceInfo> _outputDecices;

        public List<DeviceInfo> OutputDevices
        {
            get { return _outputDecices; }
            set { _outputDecices = value; }
        }

        private List<DeviceInfo> _inputDecices;

        public List<DeviceInfo> InputDevices
        {
            get { return _inputDecices; }
            set { _inputDecices = value; }
        }

        private string[] _videoDecices;

        public string[] VideoDevices
        {
            get { return _videoDecices; }
            set { _videoDecices = value; }
        }

        private Bitmap _cameraBitmap;

        public Bitmap CameraBitmap
        {
            get { return _cameraBitmap; }
            set { _cameraBitmap = value; }
        }



        ReactiveCommand<Unit, Unit> SpeedTestCommand { get; }


        public IObservable<bool> isSpeedTestRunning => SpeedTestCommand.IsExecuting;


        Func<ChartPoint, string> SpeedFormatter = (x) => Math.Truncate(x.PrimaryValue * 100) / 100 + "Mbs/s";
        public SystemRequirmentsViewModel(IScreen screen)
        {
            HostScreen = screen;
            
            initChart();

            // camera initialisation is done in the backing code of the view for now
            //initCameraPreview();

            SpeedTestCommand = ReactiveCommand.CreateFromTask(async () => { 
                InternetSpeed.Value = await getInternetSpeed(); 
            });

            OutputDevices = getOutputDevices();

            InputDevices = getInputAudioDevices();

            VideoDevices = CameraHelper.FindDevices();
        }

        void initChart()
        {
            InternetSpeed = new ObservableValue { Value = 0 };
            ChartBuilder = new GaugeBuilder()
            {
                LabelFormatter = SpeedFormatter,
                OffsetRadius = 5,
                LabelsPosition = PolarLabelsPosition.ChartCenter,
                LabelsSize = 25,
            }.AddValue(InternetSpeed, "Internet Speed").BuildSeries();
        }

        void initCameraPreview()
        {
            // [How to use]
            // check USB camera is available.
            string[] devices = CameraHelper.FindDevices();
            if (devices.Length == 0) return; // no camera.

            // check format.
            int cameraIndex = 0;
            CameraHelper.VideoFormat[] formats = CameraHelper.GetVideoFormat(cameraIndex);
            for (int i = 0; i < formats.Length; i++) Console.WriteLine("{0}:{1}", i, formats[i]);

            // create usb camera and start.
            var camera = new CameraHelper(cameraIndex, formats[0]);
            camera.Start();

            // get image.
            // Immediately after starting the USB camera,
            // GetBitmap() fails because image buffer is not prepared yet.
            var bmp = camera.GetBitmap();

            //// show image in PictureBox.
            var timer = new System.Timers.Timer(100);
            timer.Elapsed += (s, ev) =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    CameraBitmap = camera.GetBitmap();
                });
            };
            timer.Start();
        }
    }
}
