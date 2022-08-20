using Avalonia;

namespace Stellarity.UserControls;

public class NavigationViewItem : Aura.UI.Controls.Navigation.NavigationViewItem
{
    public static readonly StyledProperty<string> IconValueProperty =
        AvaloniaProperty.Register<NavigationViewItem, string>(nameof(IconValue));

    public string IconValue
    {
        get => GetValue(IconValueProperty);
        set => SetValue(IconValueProperty, value);
    }
}