using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DesktopClient.CustomControls
{
    
    public partial class Card : UserControl
    {
        public static readonly StyledProperty<object> AdditionalContentProperty =
    AvaloniaProperty.Register<Card, object>(nameof(Content));

        public object AdditionalContent
        {
            get { return GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }

        public Card()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
