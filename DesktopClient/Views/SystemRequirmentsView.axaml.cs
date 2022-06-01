using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.ViewModels;

namespace DesktopClient.Views
{
    public partial class SystemRequirmentsView : ReactiveUserControl<SystemRequirmentsViewModel>
    {
        public SystemRequirmentsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
