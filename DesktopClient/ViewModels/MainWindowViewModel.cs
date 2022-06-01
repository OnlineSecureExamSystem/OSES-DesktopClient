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
            Monitor = new SystemMonitor();


            // navigating to the main view
            //Router.Navigate.Execute(new StepManagerViewModel(this));
            Router.Navigate.Execute(new EnterCodeViewModel(this, new StepManagerViewModel(this), this));


            // starting the websocket server used for streaming
            //StreamingHelper streamingHelper = new StreamingHelper();
            //streamingHelper.InitWebsocket();

            // starting system monitoring threads 
            //Monitor.StartMonitoring();
            //Monitor.StartScreenMonitoring();
        }
    }
}
