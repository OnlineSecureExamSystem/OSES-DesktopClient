using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Presenters;
using DesktopClient.CustomControls;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using static DesktopClient.Views.StepManagerView;

namespace DesktopClient.ViewModels
{
    public class StepManagerViewModel : ViewModelBase, IScreen, IRoutableViewModel  
    {

        public RoutingState Router { get; }

        public string? UrlPathSegment => "/StepManager";

        public IScreen HostScreen { get; }

        public StepManagerViewModel(IScreen screen)
        {
            HostScreen = screen;

            Router = new RoutingState();

            Router.Navigate.Execute(new LoginFormViewModel(this));
        }
    }
}
