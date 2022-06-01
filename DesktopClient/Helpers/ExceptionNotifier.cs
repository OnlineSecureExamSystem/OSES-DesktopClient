using Avalonia.Controls.Notifications;
using DesktopClient.Models.DataTransferObjects;
using DesktopClient.Views;
using Newtonsoft.Json;
using System;

namespace DesktopClient.Helpers
{
    public static class ExceptionNotifier
    {
        public static void NotifyError(string message, bool isApi = false)
        {
            if (isApi)
            {
                NodeJsApiResponseObject response = new NodeJsApiResponseObject();
                response = JsonConvert.DeserializeObject<NodeJsApiResponseObject>(message);
                MainWindow.WindowNotificationManager.Show(new Notification("Error",
                                              "Error message :" + response.message,
                                              NotificationType.Error));
            }
            else
            {
                MainWindow.WindowNotificationManager.Show(new Notification("Error",
                                              message,
                                              NotificationType.Error));
            }
        }

        public static void NotifyWarning(string message)
        {
            MainWindow.WindowNotificationManager.Show(new Notification("Warning",
                                              message,
                                              NotificationType.Warning));
        }

        public static void NotifyWarningClick(string message, Action onCLick)
        {
            MainWindow.WindowNotificationManager.Show(new Notification("Warning",
                                              message,
                                              NotificationType.Warning, onClick: onCLick));
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
