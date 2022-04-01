using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.CustomControls.StepCircle;
using DesktopClient.ViewModels;
using ReactiveUI;
using System;

namespace DesktopClient.Views
{
    public partial class StepManagerView : ReactiveUserControl<StepManagerViewModel>
    {
        public StepManagerView()
        {
            this.WhenActivated(disposables => { });
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
