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

            await AddGame.InitializeViewModelAsync(new AddGameViewModel());
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


    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}