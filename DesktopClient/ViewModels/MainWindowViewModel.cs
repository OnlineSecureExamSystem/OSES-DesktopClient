using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Data;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Text;
using System.Text.RegularExpressions;
using static DesktopClient.Views.MainWindow;

namespace DesktopClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        public RoutingState Router { get; }

        public MainWindowViewModel()
        {
            Router = new RoutingState();
            Router.Navigate.Execute(new ExamRoomViewModel(this));
        }
    }
}
