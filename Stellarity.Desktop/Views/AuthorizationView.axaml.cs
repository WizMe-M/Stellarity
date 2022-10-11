using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using ReactiveUI;
using Stellarity.Desktop.ViewModels;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Services;

namespace Stellarity.Desktop.Views;

public partial class AuthorizationView : ReactiveWindow<AuthorizationViewModel>
{
    public AuthorizationView()
    {
        var dialog = DiContainingService.Kernel.Get<IDialogService>();
        var accounting = DiContainingService.Kernel.Get<AccountingService>();
        ViewModel = new AuthorizationViewModel(dialog, accounting);

        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        DragBorder = this.GetControl<Border>(nameof(DragBorder));
        DragBorder.PointerPressed += (_, e) => BeginMoveDrag(e);

        this.WhenActivated(async d => { await ViewModel.TryAutoLogInAsync(); });
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void MinimizeButton_OnClick(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e) => Close();
}