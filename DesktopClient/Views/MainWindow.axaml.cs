using Avalonia.Controls;
using System;
using DesktopClient.Helpers;
using Avalonia.Controls.Notifications;
using DesktopClient.ViewModels;

namespace DesktopClient.Views
{
    public partial class MainWindow : Window
    {
        public static WindowNotificationManager? windowNotificationManager;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            windowNotificationManager = new WindowNotificationManager(this)
            {
                MaxItems = 3,
            };
        }



        //setting the main window affinity as soon as the window is loaded
        public void Loaded(object sender, EventArgs e)
        {
            IntPtr handle = PlatformImpl.Handle.Handle;
            //WindowShield.SetWindowDisplayAffinity(handle, WindowShield.WDA_EXCLUDEFROMCAPTURE);
        }
    }
}