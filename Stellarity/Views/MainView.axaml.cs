using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using ReactiveUI;
using Stellarity.Services.Accounting;
using Stellarity.TemplatedControls.Navigation;
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
            EditProfile.Content =
                new EditProfileView(new EditProfileViewModel(dialogService, ViewModel!, accountingService.AuthorizedAccount!));

            Shop.Content = new GameShopView(new GameShopViewModel());
        });

        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        DragBorder = this.GetControl<Border>(nameof(DragBorder));
        DragBorder.PointerPressed += (_, e) => BeginMoveDrag(e);

        MyProfile = this.GetControl<MyProfileView>(nameof(MyProfile));
        EditProfile = this.GetControl<NavigationViewItem>(nameof(EditProfile));
        Shop = this.GetControl<NavigationViewItem>(nameof(Shop));
    }


    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}