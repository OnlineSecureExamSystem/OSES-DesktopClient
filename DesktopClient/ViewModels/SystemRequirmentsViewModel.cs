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
using static DesktopClient.Helpers.DataValidator;
using DesktopClient.Helpers;
using Avalonia.Threading;
using Avalonia.Media.Imaging;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public class SystemRequirmentsViewModel : ViewModelBase, IRoutableViewModel
    {
        public Task InitTask { get; private set; }

        public string? UrlPathSegment => "/SystemRequirments";

        public IScreen HostScreen { get; }

        IEnumerable<ISeries> ChartBuilder { get; set; }
        
        private ObservableValue _internetSpeed;

        public ObservableValue InternetSpeed
        {
            get { return _internetSpeed; }
            set 
            { _internetSpeed = value; }
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
            set { this.RaiseAndSetIfChanged(ref _cameraBitmap, value); }
        }

        public ReactiveCommand<Unit, Unit> SpeedTestCommand { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> NextCommand { get; }

        public CameraHelper Camera { get; private set; }

        public IObservable<bool> isSpeedTestRunning => SpeedTestCommand.IsExecuting;


        Func<ChartPoint, string> SpeedFormatter = (x) => Math.Truncate(x.PrimaryValue * 100) / 100 + "Mbs/s";
        public SystemRequirmentsViewModel(IScreen screen)
        {
            HostScreen = screen;

            InitTask = Task.Run(() => init());

            var canNext = this.WhenAnyValue(x => x.InternetSpeed.Value,
                (speed) => speed > 1);

            NextCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                return HostScreen.Router.Navigate.Execute(new InformationCheckViewModel(HostScreen, Camera));
            }
            );

            SpeedTestCommand = ReactiveCommand.CreateFromTask(async () => { 
                InternetSpeed.Value = await getInternetSpeed(); 
            });
        }

        void init()
        {
            OutputDevices = getOutputDevices();
            InputDevices = getInputAudioDevices();
            VideoDevices = CameraHelper.FindDevices();
            initChart();
            initCameraPreview();
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

        async void initCameraPreview()
        {
            // check format.
            int cameraIndex = 0;
            CameraHelper.VideoFormat[] formats = CameraHelper.GetVideoFormat(cameraIndex);
            // create usb camera and start.
            Camera = new CameraHelper(cameraIndex, formats[0]);
            await Task.Run(() => Camera.Start());
            // get image.
            // Immediately after starting the USB camera,
            // GetBitmap() fails because image buffer is not prepared yet.
            var bmp = Camera.GetBitmap();
            
            //// show image in PictureBox.
            var timer = new System.Timers.Timer(100);
            timer.Elapsed += (s, ev) =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    CameraBitmap = Camera.GetBitmap();
                });
            };
            timer.Start();
        }
    }
}
