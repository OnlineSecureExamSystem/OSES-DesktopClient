using DesktopClient.Helpers;
using ReactiveUI;

namespace DesktopClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        public RoutingState Router { get; }

        public MainWindowViewModel()
        {
            Router = new RoutingState();
            //Router.Navigate.Execute(new StepManagerViewModel(this));
            Router.Navigate.Execute(new WaitingRoomViewModel(this, new StepManagerViewModel(this), this));

            SystemMonitor systemMonitor = new SystemMonitor();
            if (systemMonitor.IsMonitoring)
                return;
            systemMonitor.StartMonitoring();
        }
    }
}
