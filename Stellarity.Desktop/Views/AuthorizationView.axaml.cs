﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Ninject;
using Stellarity.Desktop.ViewModels;
using Stellarity.Services;

namespace Stellarity.Desktop.Views;

public partial class AuthorizationView : ReactiveWindow<AuthorizationViewModel>
{
    public AuthorizationView()
    {
        ViewModel = DiContainingService.Kernel.Get<AuthorizationViewModel>();
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        DragBorder = this.GetControl<Border>(nameof(DragBorder));
        DragBorder.PointerPressed += (_, e) => BeginMoveDrag(e);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void MinimizeButton_OnClick(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e) => Close();
}