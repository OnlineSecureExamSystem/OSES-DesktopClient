using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.ViewModels;

namespace DesktopClient.Views
{
    public partial class ChooseVerificationMethodView : ReactiveUserControl<ChooseVerificationMethodViewModel>
    {
        public ChooseVerificationMethodView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
