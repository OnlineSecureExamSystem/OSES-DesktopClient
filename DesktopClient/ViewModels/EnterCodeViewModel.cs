using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using DesktopClient.Helpers;
using DesktopClient.Models;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using DesktopClient.CustomControls.StepCircle;

namespace DesktopClient.ViewModels
{
    public class EnterCodeViewModel : ViewModelBase, IRoutableViewModel
    {
        #region Properties
        private string? _code;

        public string? Code
        {
            get => _code;
            set
            {
                value?.IsValid(DataTypes.Code);
                this.RaiseAndSetIfChanged(ref _code, value);
            }
        }
        #endregion

         ReactiveCommand<Unit, Unit> EnterCommand { get; }

        ReactiveCommand<Unit, Unit> NavigateBack { get; }

        IObservable<bool> IsExecuting => EnterCommand.IsExecuting;

        public string? UrlPathSegment => "enter_code_path";

        public IScreen HostScreen { get; }

        public StepManagerViewModel StepManager { get; }

        public MainWindowViewModel MainWindowp { get; }



        public EnterCodeViewModel(IScreen screen, StepManagerViewModel stepManager, MainWindowViewModel mainWindowp)
        {
            HostScreen = screen;
            StepManager = stepManager;
            MainWindowp = mainWindowp;


            var canEnter = this.WhenAnyValue(
                x => x.Code,
                (code) =>
                    !string.IsNullOrEmpty(code)
                );

            EnterCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                SystemRequirmentsViewModel vm = new SystemRequirmentsViewModel(screen, StepManager, MainWindowp);
                await vm.InitTask;
                screen.Router.Navigate.Execute(vm);
                StepManager.ExamCodeCtrl = new Done();
                StepManager.SystemCheckCtrl = new Running();
            }, canEnter);

            EnterCommand.ThrownExceptions.Subscribe(x =>
                    MainWindow.WindowNotificationManager?
                        .Show(new Avalonia.Controls.Notifications.Notification(
                            "Error",
                            x.Message,
                            NotificationType.Error)));

            NavigateBack = ReactiveCommand.Create(() => { screen.Router.NavigateBack.Execute(); });
        }



    }
}
