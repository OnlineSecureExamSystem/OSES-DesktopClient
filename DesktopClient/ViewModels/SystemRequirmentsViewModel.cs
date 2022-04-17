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
using System.Threading.Tasks;
using DesktopClient.CustomControls.StepCircle;
using DesktopClient.Views;
using Avalonia.Controls.Notifications;

namespace DesktopClient.ViewModels
{
    public class SystemRequirmentsViewModel : ViewModelBase, IRoutableViewModel
    {

        #region Properties
        private ObservableValue _internetSpeed ;

        public ObservableValue InternetSpeed
        {
            get { return _internetSpeed; }
            set => this.RaiseAndSetIfChanged(ref _internetSpeed, value);
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
        #endregion
        
        public Task InitTask { get; private set; }

        public string? UrlPathSegment => "/SystemRequirments";

        public IScreen HostScreen { get; }

        IEnumerable<ISeries> ChartBuilder { get; set; }

        public ReactiveCommand<Unit, Unit> SpeedTestCommand { get; }
        
        public ReactiveCommand<Unit, Unit> NextCommand { get; }

        public ReactiveCommand<Unit, Unit> NavigateBack { get; }

        public CameraHelper Camera { get; private set; }

        public IObservable<bool> isSpeedTestRunning => SpeedTestCommand.IsExecuting;
        
        Func<ChartPoint, string> SpeedFormatter = (x) => Math.Truncate(x.PrimaryValue * 100) / 100 + "Mbs/s";

        public StepManagerViewModel StepManager { get; }
        
        public SystemRequirmentsViewModel(IScreen screen, StepManagerViewModel stepManager)
        {
            HostScreen = screen;
            StepManager = stepManager;

            InitTask = Task.Run(() => init());

            var canNext = this.WhenAnyValue(x => x.InternetSpeed.Value,
                (speed) => speed > 1);

            NextCommand = ReactiveCommand.Create(() =>
            {
                StepManager.SystemCheckCtrl = new Done();
                StepManager.InfoCheckCtrl = new Running();
                HostScreen.Router.Navigate.Execute(new InformationCheckViewModel(HostScreen, Camera, StepManager));
            });

            SpeedTestCommand = ReactiveCommand.CreateFromTask(InternetSpeedTest);

            NavigateBack = ReactiveCommand.Create(() =>
            {
                HostScreen.Router.NavigateBack.Execute();
            });
        }

        private async Task<Unit> InternetSpeedTest()
        {
            try
            {
                InternetSpeed.Value = await getInternetSpeed();
                return Unit.Default;
            }
            catch (Exception e)
            {
                MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error",
                  "There is no internet connection, try again later",
                  NotificationType.Error));
                return Unit.Default;
            }
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
