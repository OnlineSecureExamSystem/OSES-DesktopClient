using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DesktopClient.CustomControls.StepCircle
{
    public partial class Running : UserControl
    {
        public Running()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
