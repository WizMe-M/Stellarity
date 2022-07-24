using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Stellarity.UserControls;

public partial class HamburgerMenu : UserControl
{
    public HamburgerMenu()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}