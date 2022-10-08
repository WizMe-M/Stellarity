using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using Ninject.Parameters;
using ReactiveUI;
using Stellarity.Desktop.TemplatedControls.Navigation;
using Stellarity.Desktop.ViewModels;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Desktop.Views.Pages;
using Stellarity.Services;
using Stellarity.Services.Accounting;

namespace Stellarity.Desktop.Views;

public partial class MainView : ReactiveWindow<MainViewModel>
{
    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public MainView()
    {
        this.WhenActivated(async d =>
        {
            var myProfileViewModel = DiContainingService.Kernel.Get<MyProfileViewModel>();
            await MyProfile.InitializeViewModelAsync(myProfileViewModel);

            var dialog = DiContainingService.Kernel.Get<IDialogService>();
            var accounting = DiContainingService.Kernel.Get<AccountingService>();
            var editProfileViewModel = new EditProfileViewModel(dialog, accounting, ViewModel!);
            await EditProfile.InitializeViewModelAsync(editProfileViewModel);

            var shopViewModel = new GameShopViewModel(accounting, ViewModel!, NavView.Navigator, dialog);
            await Shop.InitializeViewModelAsync(shopViewModel);
        });

        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        DragBorder = this.GetControl<Border>(nameof(DragBorder));
        DragBorder.PointerPressed += (_, e) => BeginMoveDrag(e);

        NavView = this.GetControl<NavigationView>(nameof(NavView));
        foreach (var logicalChild in NavView.GetLogicalChildren())
        {
            if (logicalChild is not NavigationViewItem item) return;
            item.Initialize(NavView.Navigator);
        }

        MyProfile = this.GetControl<MyProfileView>(nameof(MyProfile));
        EditProfile = this.GetControl<EditProfileView>(nameof(EditProfile));
        Shop = this.GetControl<GameShopView>(nameof(Shop));
    }
}