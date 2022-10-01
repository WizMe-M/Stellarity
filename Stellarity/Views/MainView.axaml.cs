using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Ninject;
using Ninject.Parameters;
using ReactiveUI;
using Stellarity.ViewModels;
using Stellarity.ViewModels.Pages;
using Stellarity.Views.Pages;

namespace Stellarity.Views;

public partial class MainView : ReactiveWindow<MainViewModel>
{
    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public MainView()
    {
        this.WhenActivated(async d =>
        {
            var myProfileViewModel = App.Current.DiContainer.Get<MyProfileViewModel>();
            await MyProfile.InitializeViewModelAsync(myProfileViewModel);

            var editProfileViewModel = App.Current.DiContainer.Get<EditProfileViewModel>(
                new Parameter("windowOwner", ViewModel, false));
            await EditProfile.InitializeViewModelAsync(editProfileViewModel);

            await Shop.InitializeViewModelAsync(new GameShopViewModel());

            var addGameViewModel = App.Current.DiContainer.Get<AddGameViewModel>(
                new Parameter("windowOwner", ViewModel, false));
            await AddGame.InitializeViewModelAsync(addGameViewModel);
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
        AddGame = this.GetControl<AddGameView>(nameof(AddGame));
    }
}