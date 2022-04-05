using Avalonia.Controls;
using System;
using Avalonia.Controls.Notifications;
using DesktopClient.ViewModels;
using Avalonia.Markup.Xaml;
using DesktopClient.Helpers;

namespace DesktopClient.Views
{
    public partial class MainWindow : Window
    {
        public static WindowNotificationManager? WindowNotificationManager;

        public MainWindow()
        {

            InitializeComponent();
            
            WindowNotificationManager = new WindowNotificationManager(this)
            {
                MaxItems = 3,
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        //setting the main window affinity as soon as the window is loaded
        public void Loaded(object sender, EventArgs e)
        {
            IntPtr handle = PlatformImpl.Handle.Handle;
            //WindowShield.SetWindowDisplayAffinity(handle, WindowShield.WDA_EXCLUDEFROMCAPTURE);
        }
    }
}