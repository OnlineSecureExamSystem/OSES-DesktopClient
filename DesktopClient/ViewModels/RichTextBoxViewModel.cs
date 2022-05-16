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




            SubmitCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var text = await Content.EvaluateScriptFunction<string>("submit");
            });
        }

    }
}
