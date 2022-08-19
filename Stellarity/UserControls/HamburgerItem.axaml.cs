using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Stellarity.Basic;

namespace Stellarity.UserControls;

/// <summary>
/// An item of <see cref="HamburgerMenu"/>
/// </summary>
public class HamburgerItem : TabItem
{
    #region Hamburger items' click

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

    #endregion


    #region Page of hamburger item

    public static readonly AvaloniaProperty PageProperty =
        AvaloniaProperty.Register<HamburgerItem, PageViewModel>(nameof(Page), inherits: true,
            defaultBindingMode: BindingMode.OneTime);

    /// <summary>
    /// Page-content of a hamburger item
    /// </summary>
    /// <remarks>There is a contract that item's content will inherit <see cref="PageViewModel"/></remarks>
    public PageViewModel? Page
    {
        get => (PageViewModel?)GetValue(PageProperty);
        set => SetValue(PageProperty, value);
    }

    #endregion
}