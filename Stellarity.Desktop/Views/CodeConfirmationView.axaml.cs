using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Stellarity.Desktop.Views;

public partial class CodeConfirmationView : Window
{
    public CodeConfirmationView() => AvaloniaXamlLoader.Load(this);

    private void Cancel_OnClick(object? sender, RoutedEventArgs e) => Close();
}