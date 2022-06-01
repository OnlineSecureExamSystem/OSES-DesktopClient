using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DesktopClient.CustomControls
{
    public partial class LoadingScreen : UserControl
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
