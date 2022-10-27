using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Stellarity.Desktop.Navigation.Event;

namespace Stellarity.Desktop.TemplatedControls.Navigation;

public class NavigationViewItem : TabItem
{
    private NavigationPublisher? _navigator;

    public static readonly StyledProperty<string> IconValueProperty =
        AvaloniaProperty.Register<NavigationViewItem, string>(nameof(IconValue));

    public static readonly StyledProperty<IContentPage> ContentPageProperty =
        AvaloniaProperty.Register<NavigationViewItem, IContentPage>(nameof(ContentPage));

    public string IconValue
    {
        get => GetValue(IconValueProperty);
        set => SetValue(IconValueProperty, value);
    }

    public IContentPage? ContentPage
    {
        get => GetValue(ContentPageProperty);
        set => SetValue(ContentPageProperty!, value);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var page = ContentPage ?? new DefaultContent();
        _navigator?.RaiseNavigated(this,
            IsSelected ? NavigatedEventArgs.ClearAndPush(page) : NavigatedEventArgs.Push(page));
    }

    public void Initialize(NavigationPublisher navigator) => _navigator = navigator;
}