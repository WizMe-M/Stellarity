using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Stellarity.UserControls;

public partial class HamburgerItem : UserControl
{
    public HamburgerItem()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}