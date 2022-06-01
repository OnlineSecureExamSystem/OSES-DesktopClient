using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using DesktopClient.Services;
using DesktopClient.ViewModels;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DesktopClient.Helpers
{
    public class SystemMonitor
    {
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        private DispatcherTimer _timer;
        private DispatcherTimer _examTimer;
        private DispatcherTimer _internetTimer;
        private DispatcherTimer _screenTimer;
        int connectionCutCount = 0;
        public bool IsMonitoring { get; private set; }


        public static bool IsMessageBoxOpen { get; private set; }
        public static bool IsInExamRoom { get; set; }

        ExamService examService = new ExamService();

        public ExamRoomViewModel ExamRoom { get; set; }

        private List<Process> processes = new List<Process>();

        private List<int> forbiddenPrecessIDs = new List<int>();

        private List<string> processToClose = new List<string>();

        private List<string> forbiddenPrecessNames = new List<string>()
        {
            // browsers process names
            "chrome",
            "firefox",
            "opera",
            "iexplore",
            "safari",
            "chromium",
            //"msedge",
            
            // comunication process names ,zooom
            "skype",
            "Discord",
            "Telegram",
            "WhatsApp",
            "SkypeForBusiness",
            "Microsoft Teams",

            // text editors process
            "notepad",
            "notepad++",
            "sublime_text",
            "atom",
            "code",
            "visual studio code",
            "visual studio",
            "WINWORD",
            "EXCEL",
            "POWERPNT",
            "OUTLOOK",
            "WINWORD",
            //"HxOutlook",
            
            // recording process names
            // no need to close them because they cant record anything
            
            // other process names
            "mspaint",
        };

        public SystemMonitor()
        {
            IsInExamRoom = false;
            IsMessageBoxOpen = false;
        }

        public SystemMonitor(ExamRoomViewModel examRoom)
        {
            ExamRoom = examRoom;
            IsInExamRoom = false;
            IsMessageBoxOpen = false;
        }
        public void StartMonitoring()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += timer_Elapsed;
            _timer.Start();
            IsMonitoring = true;
        }


        public void StartInternetMonitoring()
        {
            _internetTimer = new DispatcherTimer();
            _internetTimer.Interval = TimeSpan.FromSeconds(5);
            _internetTimer.Tick += internetTimer_Elapsed;
            _internetTimer.Start();
        }

        public void StartScreenMonitoring()
        {
            _screenTimer = new DispatcherTimer();
            _screenTimer.Interval = TimeSpan.FromSeconds(5);
            _screenTimer.Tick += screenTimer_Elapsed;
            _screenTimer.Start();
        }



        public void StartExamMonitoring()
        {
            _examTimer = new DispatcherTimer();
            _examTimer.Interval = TimeSpan.FromSeconds(5);
            _examTimer.Tick += examTimer_Elapsed;
            _examTimer.Start();
            IsMonitoring = true;
        }

        private async void screenTimer_Elapsed(object sender, EventArgs e)
        {
            int screenCount = GetSystemMetrics(80);
            if (screenCount > 1)
            {
                if (!IsMessageBoxOpen)
                {
                    if (IsInExamRoom)
                    {
                        IsMessageBoxOpen = true;
                        var endMessageBox = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {

                            ButtonDefinitions = new[] { new ButtonDefinition { Name = "Ok", IsDefault = true }, },
                            ContentTitle = "Warning",
                            ContentMessage = "You can't use more than one screen in the exam room,\n disconnect the other screens or you will be kicked",
                            Icon = Icon.Info,
                            Topmost = true,
                            CanResize = false,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        });
                        var r2 = await endMessageBox.ShowDialog(getMainWindow());
                    }
                    else
                    {
                        ExceptionNotifier.NotifyWarning("You have more than one screen connected to your computer.\n" +
                        "Please connect only one screen to your computer.");
                    }
                }
            }
        }

        private async void internetTimer_Elapsed(object sender, EventArgs e)
        {
            var answers = await ExamRoom.getExamAnswers();
            bool result = await examService.SendExamAnswers(answers);
            if (result)
            {
                //ExceptionNotifier.NotifySuccess("Answers Saved");
            }
            else
            {
                connectionCutCount++;
                if (!IsMessageBoxOpen)
                {
                    if (connectionCutCount > 2)
                    {
                        IsMessageBoxOpen = true;
                        var endMessageBox = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {

                            ButtonDefinitions = new[] { new ButtonDefinition { Name = "Ok", IsDefault = true }, },
                            ContentTitle = "Goodbey",
                            ContentMessage = "Your internet connection is cutted alote,\n we had to kick you out of the exam room",
                            Icon = Icon.Info,
                            Topmost = true,
                            CanResize = false,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        });
                        var r2 = await endMessageBox.ShowDialog(getMainWindow());
                        Environment.Exit(0);
                    }

                    var messageBox = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {

                        ButtonDefinitions = new[] { new ButtonDefinition { Name = "Retry", IsDefault = true },
                                                new ButtonDefinition { Name = "Exit Exam", IsDefault = false }},
                        ContentTitle = "Warning",
                        ContentMessage = "You have no internet connection,\n If you exit now your answers will be sent automaticaly \n but you cant join the exam session again",
                        Icon = Icon.Warning,
                        Topmost = true,
                        CanResize = false,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    });
                    IsMessageBoxOpen = true;
                    var r = await messageBox.ShowDialog(getMainWindow());
                    if (r == "Retry")
                    {
                        bool r1 = await examService.SendExamAnswers(answers);
                        if (r1)
                        {
                            ExceptionNotifier.NotifySuccess("Connection Restored");
                        }
                        else
                        {
                            ExceptionNotifier.NotifyError("No Internet Connection");
                        }
                        IsMessageBoxOpen = false;
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }

        private async void examTimer_Elapsed(object sender, EventArgs e)
        {
            processToClose.Clear();

            foreach (var process in Process.GetProcesses())
            {
                if (forbiddenPrecessNames.Contains(process.ProcessName))
                {
                    processToClose.Add(process.ProcessName);
                }
            }

            if (processToClose.Count != 0)
            {
                // remove duplicates
                processToClose = new List<string>(new HashSet<string>(processToClose));

                if (!IsMessageBoxOpen)
                {
                    IsMessageBoxOpen = true;
                    ExamRoom.BackgroundOn.Execute().Subscribe();
                    var messageBox = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {

                        ButtonDefinitions = new[] { new ButtonDefinition { Name = "Next", IsDefault = true } },
                        ContentTitle = "Warning",
                        ContentMessage = "We detected that you have some programs that are not allowed during an exam.\nIf you don't close them , you will be marked as cheater and reported to the proctor",
                        Icon = Icon.Warning,
                        Topmost = true,
                        CanResize = false,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    });



                    var result = await messageBox.ShowDialog(getMainWindow());
                    if (result == "Next")
                    {
                        var messageBoxClose = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                        {
                            new ButtonDefinition{Name = "Confirm", IsDefault = true},
                            },
                            ContentTitle = "Info",
                            ContentMessage = "The following processes going to be closed:\n" + string.Join("\n", processToClose),
                            Icon = Icon.Info,
                            Topmost = true,
                            CanResize = false,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        });

                        var resultC = await messageBoxClose.ShowDialog(getMainWindow());
                        if (resultC == "Confirm")
                        {
                            KillProcesses();
                            IsMessageBoxOpen = false;
                        }
                        ExamRoom.BackgroundOff.Execute().Subscribe();
                    }
                }
            }
        }

        public void StopMonitoring()
        {
            _timer.Stop();
            IsMonitoring = false;
        }

        private async void timer_Elapsed(object sender, EventArgs e)
        {
            processToClose.Clear();

            foreach (var process in Process.GetProcesses())
            {
                if (forbiddenPrecessNames.Contains(process.ProcessName))
                {
                    processToClose.Add(process.ProcessName);
                }
            }

            if (processToClose.Count != 0)
            {
                // remove duplicates
                processToClose = new List<string>(new HashSet<string>(processToClose));
                if (IsInExamRoom)
                {
                    if (!IsMessageBoxOpen)
                    {
                        IsMessageBoxOpen = true;
                        var messageBox = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {

                            ButtonDefinitions = new[] { new ButtonDefinition { Name = "Next", IsDefault = true } },
                            ContentTitle = "Warning",
                            ContentMessage = "We detected that you have some programs that are not allowed during an exam.\nIf you don't close them , you will be marked as cheater and reported to the proctor",
                            Icon = Icon.Warning,
                            Topmost = true,
                            SystemDecorations = SystemDecorations.BorderOnly,
                            CanResize = false,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        });
                        var result = await messageBox.Show();
                        if (result == "Next")
                        {
                            var messageBoxClose = MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions = new[]
                            {
                            new ButtonDefinition{Name = "Confirm", IsDefault = true},
                            },
                                ContentTitle = "Info",
                                ContentMessage = "The following processes going to be closed:\n" + string.Join("\n", processToClose),
                                Icon = Icon.Info,
                                Topmost = true,
                                SystemDecorations = SystemDecorations.BorderOnly,
                                CanResize = false,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen
                            });

                            var resultC = await messageBoxClose.Show();
                            if (resultC == "Confirm")
                            {
                                KillProcesses();
                                ExamRoom.MsgBoxBackground = null;
                                IsMessageBoxOpen = false;
                            }
                        }
                    }
                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        ExceptionNotifier.NotifyWarningClick("These apps are not allowed to run on the desktop : " + string.Join(", ", processToClose) + "\n Click here to close them", KillProcesses);
                    });
                }

            }
        }

        void KillProcesses()
        {
            foreach (var process in processToClose)
            {
                foreach (var p in Process.GetProcessesByName(process))
                {
                    if (p.ProcessName == process)
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch (Exception e)
                        {

                            ExceptionNotifier.NotifyError(e.Message);
                        }

                    }
                }
            }

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ExceptionNotifier.NotifyInfo("The following processes where closed!" + string.Join(", ", processToClose));
                processToClose.Clear();
            });
        }

        void UpdateProcesses()
        {
            foreach (var process in Process.GetProcesses())
            {
                if (!processes.Contains(process))
                {
                    processes.Add(process);
                }
            }
        }

        Window getMainWindow()
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            return null;
        }
    }
}
