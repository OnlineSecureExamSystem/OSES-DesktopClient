using Avalonia.Controls.Notifications;
using DesktopClient.Views;

namespace DesktopClient.Helpers
{
    public static class ExceptionNotifier
    {
        public static void NotifyError(string message)
        {
            MainWindow.WindowNotificationManager.Show(new Notification("Error",
                                              message,
                                              NotificationType.Error));
        }

        public static void NotifyWarning(string message)
        {
            MainWindow.WindowNotificationManager.Show(new Notification("Warning",
                                              message,
                                              NotificationType.Warning));
        }

        public static void NotifyInfo(string message)
        {
            MainWindow.WindowNotificationManager.Show(new Notification("Info",
                                              message,
                                              NotificationType.Information));
        }

        public static void NotifySuccess(string message)
        {
            MainWindow.WindowNotificationManager.Show(new Notification("Success",
                                              message,
                                              NotificationType.Success));
        }
    }
}
