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
using DesktopClient.CustomControls.StepCircle;
using static DesktopClient.Views.StepManagerView;

namespace DesktopClient.ViewModels
{
    public class StepManagerViewModel : ViewModelBase, IScreen, IRoutableViewModel  
    {
        #region Properties
        private Control _loginCtrl = new Running();

        public Control LoginCtrl
        {
            get { return _loginCtrl; }
            set { this.RaiseAndSetIfChanged(ref _loginCtrl, value); }
        }

        private Control _examCodeCtrl = new Step2();

        public Control ExamCodeCtrl
        {
            get { return _examCodeCtrl; }
            set { this.RaiseAndSetIfChanged(ref _examCodeCtrl, value); }
        }

        private Control _systemCheckCtrl = new Step3();

        public Control SystemCheckCtrl
        {
            get { return _systemCheckCtrl; }
            set { this.RaiseAndSetIfChanged(ref _systemCheckCtrl, value); }
        }

        private Control _infoCheckCtrl = new Step4();
        
        public Control InfoCheckCtrl
        {
            get { return _infoCheckCtrl; }
            set { this.RaiseAndSetIfChanged(ref _infoCheckCtrl, value); }
        }

        private Control _startExamCtrl = new Step5();

        public Control StartExamCtrl
        {
            get { return _startExamCtrl; }
            set { this.RaiseAndSetIfChanged(ref _startExamCtrl, value); }
        }
        #endregion

        public RoutingState Router { get; }

        public string? UrlPathSegment => "/StepManager";

        public IScreen HostScreen { get; }

        public StepManagerViewModel(IScreen screen)
        {
            

            HostScreen = screen;

            Router = new RoutingState();
            
            Router.Navigate.Execute(new LoginFormViewModel(this, this));
        }
    }
}
