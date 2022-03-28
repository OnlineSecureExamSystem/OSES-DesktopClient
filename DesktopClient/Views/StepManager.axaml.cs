using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DesktopClient.CustomControls.StepCircle;

namespace DesktopClient.Views
{
    public partial class StepManager : UserControl
    {
        public static StepManager? stepManager;
        public StepManager()
        {
            InitializeComponent();
            stepManager = this;
            DataContext = stepManager;
            //ContentControl ellipse1 = this.FindControl<ContentControl>("ellipse1");
            //ellipse1.Content = new Running();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
