using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using DesktopClient.Helpers;
using DesktopClient.ViewModels;
using System;

namespace DesktopClient.Views
{
    public partial class SystemRequirmentsView : ReactiveUserControl<SystemRequirmentsViewModel>
    {
        public SystemRequirmentsView()
        {
            InitializeComponent();
            initCameraPreview();
        }

        private void initCameraPreview()
        {
            var image = this.FindControl<Image>("preview");

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
                    if (image is not null)
                        image.Source = camera.GetBitmap();
                });
            };
            timer.Start();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
