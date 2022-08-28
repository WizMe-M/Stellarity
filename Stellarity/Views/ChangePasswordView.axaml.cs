using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Stellarity.Views;

public partial class ChangePasswordView : Window
{
    public ChangePasswordView()
    {
        AvaloniaXamlLoader.Load(this);
#if DEBUG
        this.AttachDevTools();
#endif
    }
}