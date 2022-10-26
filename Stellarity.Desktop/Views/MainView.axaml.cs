using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using ReactiveUI;
using Stellarity.Desktop.TemplatedControls.Navigation;
using Stellarity.Desktop.ViewModels;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Desktop.Views.Pages;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Ninject;
using Stellarity.Domain.Services;

namespace Stellarity.Desktop.Views;

public partial class MainView : ReactiveWindow<MainViewModel>
{
    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public MainView()
    {
        this.WhenActivated(async d =>
        {
            DiContainingService.Kernel
                .Bind<MainViewModel>()
                .ToConstant(ViewModel!)
                .InSingletonScope();

            var dialog = DiContainingService.Kernel.Get<IDialogService>();
            var accounting = DiContainingService.Kernel.Get<AccountingService>();

            var myProfileViewModel = DiContainingService.Kernel.Get<MyProfileViewModel>();
            MyProfile.ViewModel = myProfileViewModel;
            EditProfile.ViewModel = new EditProfileViewModel(dialog, accounting, ViewModel!);

            var shopViewModel = new GameShopViewModel(accounting, ViewModel!, NavView.Navigator, dialog);
            await Shop.InitializeViewModelAsync(shopViewModel);

            Library.ViewModel = new LibraryViewModel(NavView.Navigator);
            Community.ViewModel = new CommunityViewModel(NavView.Navigator);
        });

        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        DragBorder = this.GetControl<Border>(nameof(DragBorder));
        DragBorder.PointerPressed += (_, e) => BeginMoveDrag(e);
        MyProfile = this.GetControl<MyProfileView>(nameof(MyProfile));
        EditProfile = this.GetControl<EditProfileView>(nameof(EditProfile));
        Shop = this.GetControl<GameShopView>(nameof(Shop));
        Library = this.GetControl<LibraryView>(nameof(Library));
        Community = this.GetControl<CommunityView>(nameof(Community));

        NavView = this.GetControl<NavigationView>(nameof(NavView));
        foreach (var logicalChild in NavView.GetLogicalChildren())
        {
            if (logicalChild is not NavigationViewItem item) return;
            item.Initialize(NavView.Navigator);
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        DiContainingService.Kernel.Unbind<MainViewModel>();
    }
}