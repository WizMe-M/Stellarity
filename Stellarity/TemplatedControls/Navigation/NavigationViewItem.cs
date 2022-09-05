using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Stellarity.TemplatedControls.Navigation;

public class NavigationViewItem : TabItem
{
    public static readonly RoutedEvent<ChangeContentRequestReceivedEventArgs> ChangeContentRequestReceivedEvent =
        RoutedEvent.Register<NavigationView, ChangeContentRequestReceivedEventArgs>(
            nameof(ChangeContentRequestReceived), RoutingStrategies.Bubble);

    public static readonly StyledProperty<string> IconValueProperty =
        AvaloniaProperty.Register<NavigationViewItem, string>(nameof(IconValue));

    public static readonly StyledProperty<IContentPage> ContentPageProperty =
        AvaloniaProperty.Register<NavigationViewItem, IContentPage>(nameof(ContentPage));

    public event EventHandler<ChangeContentRequestReceivedEventArgs> ChangeContentRequestReceived
    {
        add => AddHandler(ChangeContentRequestReceivedEvent, value);
        remove => RemoveHandler((RoutedEvent)ChangeContentRequestReceivedEvent, value);
    }

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
        if (ContentPage is null) return;

        var args = IsSelected
            ? ChangeContentRequestReceivedEventArgs.ClearAndPush(ContentPage, ChangeContentRequestReceivedEvent)
            : ChangeContentRequestReceivedEventArgs.Push(ContentPage, ChangeContentRequestReceivedEvent);
        RaiseEvent(args);
    }
}