using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Stellarity.Views;

public partial class AuthorizationView : Window
{
    public AuthorizationView()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        var border = this.GetControl<Border>(nameof(DragBorder));
        border.PointerPressed += (_, e) => BeginMoveDrag(e);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void MinimizeButton_OnClick(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e) => Close();
}