using Avalonia.Controls.Notifications;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using DesktopClient.Helpers;
using DesktopClient.Models;
using DesktopClient.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public class InformationCheckViewModel : ViewModelBase, IRoutableViewModel
    {

        #region Properties

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                value.IsValid(DataTypes.Name);
                this.RaiseAndSetIfChanged(ref _firstName, value);
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                value.IsValid(DataTypes.Name);
                this.RaiseAndSetIfChanged(ref _lastName, value);
            }
        }

        private DateTimeOffset? _birthDate;

        public DateTimeOffset? BirthDate
        {
            get { return _birthDate; }
            set
            {
                value.IsValid(DataTypes.Date);
                this.RaiseAndSetIfChanged(ref _birthDate, value);
            }
        }

        private string _registrationNumber;

        public string RegistrationNumber
        {
            get { return _registrationNumber; }
            set 
            {
                value.IsValid(DataTypes.RegistrationNumber);
                this.RaiseAndSetIfChanged(ref _registrationNumber, value);
            }
        }

        private Bitmap _cameraBitmap;

        public Bitmap CameraBitmap
        {
            get { return _cameraBitmap; }
            set 
            {
                this.RaiseAndSetIfChanged(ref _cameraBitmap, value);
            }
        }

        private Bitmap _capturedFace;

        public Bitmap CapturedFace
        {
            get { return _capturedFace; }
            set
            {
                this.RaiseAndSetIfChanged(ref _capturedFace, value);
            }
        }

        private Bitmap _capturedCard;

        public Bitmap CapturedCard
        {
            get { return _capturedCard; }
            set
            {
                this.RaiseAndSetIfChanged(ref _capturedCard, value);
            }
        }

        private double _capturedFaceOpacity;

        public double CapturedFaceOpacity
        {
            get { return _capturedFaceOpacity; }
            set
            {
                this.RaiseAndSetIfChanged(ref _capturedFaceOpacity, value);
            }
        }

        private double _capturedCardOpacity;

        public double CapturedCardOpacity
        {
            get { return _capturedCardOpacity; }
            set
            {
                this.RaiseAndSetIfChanged(ref _capturedCardOpacity, value);
            }
        }

        private string _captureCardButtonText = "Capture Card";

        public string CaptureCardButtonText
        {
            get { return _captureCardButtonText; }
            set
            {
                this.RaiseAndSetIfChanged(ref _captureCardButtonText, value);
            }
        }

        private string _captureFaceButtonText = "Capture Face";

        public string CaptureFaceButtonText
        {
            get { return _captureFaceButtonText; }
            set
            {
                this.RaiseAndSetIfChanged(ref _captureFaceButtonText, value);
            }
        }

        private bool _isFaceTaking = true;

        public bool IsFaceTaking
        {
            get { return _isFaceTaking; }
            set
            {
                this.RaiseAndSetIfChanged(ref _isFaceTaking, value);
            }
        }

        private bool _isCardTaking = true;

        public bool IsCardTaking
        {
            get { return _isCardTaking; }
            set
            {
                this.RaiseAndSetIfChanged(ref _isCardTaking, value);
            }
        }
        #endregion

        public IObservable<bool> Executing => NextCommand.IsExecuting;

        public ReactiveCommand<Unit, Unit> NextCommand { get; }

        public string? UrlPathSegment => "/InformationCheck";

        public ReactiveCommand<Unit, Unit> CaptureFace { get; }
        
        public ReactiveCommand<Unit, Unit> CaptureCard { get; }
        
        public ReactiveCommand<Unit, Unit> NavigateBack { get; }

        public IScreen HostScreen { get; }

        public CameraHelper Camera { get; }


        public InformationCheckViewModel(IScreen screen, CameraHelper camera)
        {
            HostScreen = screen;
            Camera = camera;
            initCameraPreview();
            IsCardTaking = true;
            IsFaceTaking = true;

            CaptureFace = ReactiveCommand.Create(() =>
            {
                if (IsFaceTaking)
                {
                    CapturedFace = Camera.GetBitmap();
                    CapturedFaceOpacity = 1;
                    CaptureFaceButtonText = "Retake";
                    IsFaceTaking = false;
                }
                else
                {
                    CapturedFace.Dispose();
                    CapturedFaceOpacity = 0;
                    CaptureFaceButtonText = "Capture Face";
                    IsFaceTaking = true;
                }
            });

            CaptureCard = ReactiveCommand.Create(() =>
            {
                if (IsCardTaking)
                {
                    CapturedCard = Camera.GetBitmap();
                    CapturedCardOpacity = 1;
                    CaptureCardButtonText = "Retake";
                    IsCardTaking = false;
                }
                else
                {
                    CapturedCard.Dispose();
                    CapturedCardOpacity = 0;
                    CaptureCardButtonText = "Capture Card";
                    IsCardTaking = true;
                }
            });

            var canNext = this.WhenAnyValue(
                x => x.FirstName,
                x => x.LastName,
                x => x.BirthDate,
                x => x.RegistrationNumber,
                x => x.IsCardTaking,
                x => x.IsFaceTaking,
                (first, last, birth, num, cardTaking, faceTaking) =>
                    !string.IsNullOrEmpty(first) &&
                    !string.IsNullOrEmpty(last) &&
                    // !string.IsNullOrEmpty(birth.Value.ToString()) &&
                    !string.IsNullOrEmpty(num) &&
                    !cardTaking && !faceTaking
                );
            NextCommand = ReactiveCommand.Create(() =>
            {
                HostScreen.Router.Navigate.Execute(new WaitingRoomViewModel(HostScreen));
            }, canNext);

            NavigateBack = ReactiveCommand.Create(() =>
            {
                HostScreen.Router.NavigateBack.Execute();
            });

            // exception handeling
            NextCommand.ThrownExceptions.Subscribe(x =>
                      MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error",
                      x.Message,
                      NotificationType.Error)));

            CaptureCard.ThrownExceptions.Subscribe(x =>
                      MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error",
                      x.Message,
                      NotificationType.Error)));

            CaptureFace.ThrownExceptions.Subscribe(x =>
                      MainWindow.WindowNotificationManager?.Show(new Avalonia.Controls.Notifications.Notification("Error",
                      x.Message,
                      NotificationType.Error)));
        }

        void initCameraPreview()
        {
            Camera.Start();
            var bmp = Camera.GetBitmap();
            var timer = new System.Timers.Timer(100);
            timer.Elapsed += (s, ev) =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    CameraBitmap = Camera.GetBitmap();
                });
            };
            timer.Start();
        }
    }
}
