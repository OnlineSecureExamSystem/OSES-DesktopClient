using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopClient.ViewModels;

namespace DesktopClient.Views
{
    public partial class ExamRoomView : ReactiveUserControl<ExamRoomViewModel>
    {
        public ExamRoomView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
