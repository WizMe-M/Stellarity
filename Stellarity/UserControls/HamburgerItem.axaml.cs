using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Stellarity.UserControls;

/// <summary>
/// An item of <see cref="HamburgerMenu"/>
/// </summary>
public class HamburgerItem : TabItem
{
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly RoutedEvent<RoutedEventArgs> SelectedClickEvent =
        RoutedEvent.Register<HamburgerItem, RoutedEventArgs>(nameof(SelectedClick), RoutingStrategies.Bubble);
    
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly RoutedEvent<RoutedEventArgs> UnselectedClickEvent =
        RoutedEvent.Register<HamburgerItem, RoutedEventArgs>(nameof(UnselectedClick), RoutingStrategies.Bubble);

    /// <summary>
    /// Raised when the user clicks the selected <see cref="HamburgerItem"/>
    /// </summary>
    public event EventHandler<RoutedEventArgs> SelectedClick
    {
        add => AddHandler(SelectedClickEvent, value);
        remove => RemoveHandler(SelectedClickEvent, value);
    }
    
    /// <summary>
    /// Raised when the user clicks not selected <see cref="HamburgerItem"/>
    /// </summary>
    public event EventHandler<RoutedEventArgs> UnselectedClick
    {
        add => AddHandler(UnselectedClickEvent, value);
        remove => RemoveHandler(UnselectedClickEvent, value);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        var clickEventArgs = new RoutedEventArgs(IsSelected ? SelectedClickEvent : UnselectedClickEvent); 
        RaiseEvent(clickEventArgs);
    }
}