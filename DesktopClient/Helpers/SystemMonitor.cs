using Avalonia.Controls;
using Avalonia.Threading;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DesktopClient.Helpers
{
    public class SystemMonitor
    {
        private DispatcherTimer _timer;

        public bool IsMonitoring { get; private set; }

        public static bool IsMessageBoxOpen { get; private set; }
        public static bool IsInExamRoom { get; set; }

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
            "msedge",
            
            // comunication process names
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
            "HxOutlook",
            
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
        public void StartMonitoring()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += timer_Elapsed;
            _timer.Start();
            IsMonitoring = true;
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
                        p.Kill();
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

    }
}
