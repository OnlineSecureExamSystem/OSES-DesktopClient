using Avalonia;
using Avalonia.Controls;

using Avalonia.Markup.Xaml;

namespace examClientMVVM.Views
{
    public partial class LoginForm : UserControl
    {

        public LoginForm()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
