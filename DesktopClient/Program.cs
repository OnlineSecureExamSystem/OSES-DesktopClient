using Avalonia;
using Avalonia.ReactiveUI;
using DesktopClient;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using Splat;
using System;
using DesktopClient.ViewModels;
using DesktopClient.Views;
using ReactiveUI;
using System.Reflection;

namespace DesktopClient
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        [Obsolete]
        public static void Main(string[] args) => BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());

        // Avalonia configuration.
        public static AppBuilder BuildAvaloniaApp()
        {
            // registering views so the locator can resolve them when needed
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());

            
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .WithIcons(container => container
                    .Register<FontAwesomeIconProvider>());
            
        }
    }
}
