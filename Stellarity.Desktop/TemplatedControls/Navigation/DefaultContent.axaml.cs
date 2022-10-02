using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.TemplatedControls.Navigation;

public partial class DefaultContent : UserControl, IContentPage
{
    public DefaultContent() => AvaloniaXamlLoader.Load(this);
}