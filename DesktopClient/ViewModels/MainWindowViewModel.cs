using ReactiveUI;

namespace DesktopClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        public RoutingState Router { get; }

        public MainWindowViewModel()
        {
            Router = new RoutingState();
            //Router.Navigate.Execute(new RichTextBoxViewModel(this));
            Router.Navigate.Execute(new WaitingRoomViewModel(this, new StepManagerViewModel(this), this));
        }
    }
}
