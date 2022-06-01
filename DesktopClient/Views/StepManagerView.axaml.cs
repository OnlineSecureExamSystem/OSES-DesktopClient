using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.ViewModels;
using ReactiveUI;

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
