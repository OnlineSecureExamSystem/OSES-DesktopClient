using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.ViewModels;

namespace DesktopClient.Views
{
    public partial class EnterVerificationCodeView : ReactiveUserControl<EnterVerificationCodeViewModel>
    {
        public EnterVerificationCodeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
