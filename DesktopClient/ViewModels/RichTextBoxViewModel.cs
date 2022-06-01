using Avalonia;
using Avalonia.Layout;
using ReactiveUI;
using System.Reactive;
using WebViewControl;

namespace DesktopClient.ViewModels
{
    public class RichTextBoxViewModel : ViewModelBase, IRoutableViewModel
    {
        #region Properties

        // TODO: convert to a relative path

        private WebView _content;

        public WebView Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }
        #endregion
        public string? UrlPathSegment => GetType().Name;
        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Unit> SubmitCommand { get; }

        public RichTextBoxViewModel(IScreen screen)
        {
            HostScreen = screen;


            Content = new WebView()
            {
                Margin = new Thickness(5, 5, 5, 5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                MinHeight = 80,
                Address = System.AppDomain.CurrentDomain.BaseDirectory + "Assets/HtmlViews/RichTextBox.html"
            };

            SubmitCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var text = await Content.EvaluateScriptFunction<string>("submit");
            });
        }

    }
}
