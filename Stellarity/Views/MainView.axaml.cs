using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using ReactiveUI;
using Stellarity.Services.Accounting;
using Stellarity.ViewModels;
using Stellarity.ViewModels.Pages;
using Stellarity.Views.Pages;

namespace Stellarity.Views;

public partial class MainView : ReactiveWindow<MainViewModel>
{
    public MainView()
    {
        this.WhenActivated(async d =>
        {
            var accountingService = App.Current.DiContainer.Get<AccountingService>();
            await MyProfile.InitializeViewModelAsync(new MyProfileViewModel(accountingService));

            var dialogService = App.Current.DiContainer.Get<IDialogService>();
            await EditProfile.InitializeViewModelAsync(new EditProfileViewModel(dialogService, ViewModel!,
                accountingService.AuthorizedAccount!));

            await Shop.InitializeViewModelAsync(new GameShopViewModel());
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
    }


    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}