using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.ViewModels;

namespace DesktopClient.Views
{
    public partial class EnterCode : UserControl
    {
        public EnterCode()
        {
            InitializeComponent();
            DataContext = new EnterCodeViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Loaded(object sender, VisualTreeAttachmentEventArgs e)
        {
            Control border = this.FindControl<Border>("BorderToAnimate");
            border.Classes.Add("Fade");
        }
    }
}
