using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.ViewModels;
using ReactiveUI;

namespace DesktopClient.Views
{
    public partial class EnterCodeView : ReactiveUserControl<EnterCodeViewModel>
    {
        public EnterCodeView()
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
