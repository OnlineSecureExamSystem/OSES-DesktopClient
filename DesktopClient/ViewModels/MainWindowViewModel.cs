using DesktopClient.Helpers;
using ReactiveUI;

namespace DesktopClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        public RoutingState Router { get; }

        public SystemMonitor Monitor { get; private set; }

        public MainWindowViewModel()
        {
            Router = new RoutingState();
            //Router.Navigate.Execute(new StepManagerViewModel(this));
            Router.Navigate.Execute(new WaitingRoomViewModel(this, new StepManagerViewModel(this), this));

            Monitor = new SystemMonitor();

            Monitor.StartMonitoring();
        }
    }
}
