using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DesktopClient.CustomControls.StepCircle;
using DesktopClient.ViewModels;
using System;

namespace DesktopClient.Views
{
    public partial class StepManager : UserControl
    {
        public static StepManager? stepManager;
        public StepManager()
        {
            InitializeComponent();
            stepManager = this;
            DataContext = new StepManagerViewModel();
            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        

     
    }
}
