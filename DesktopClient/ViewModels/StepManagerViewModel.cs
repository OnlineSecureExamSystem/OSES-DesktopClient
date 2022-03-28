using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using static DesktopClient.Views.StepManager;

namespace DesktopClient.ViewModels
{
    public class StepManagerViewModel : ViewModelBase
    {
        private Control _page;

        public Control Page
        {
            get => _page;
            set 
            {
                this.RaiseAndSetIfChanged(ref _page, value);
            }
        }

        public ReactiveCommand<Control, Unit> NavigateCommand { get; set; }

        public StepManagerViewModel()
        {
            NavigateCommand = ReactiveCommand.Create<Control>(Navigate);
            NavigateCommand.Execute(new LoginForm()).Subscribe();
        }

        private void Navigate(Control obj)
        {
             stepManager.FindControl<ContentPresenter>("host").Content = obj;
        }
        

    }
}
