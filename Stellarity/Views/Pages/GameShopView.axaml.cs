using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.ViewModels.Pages;

namespace Stellarity.Views.Pages;

public partial class GameShopView : ReactiveUserControl<GameShopViewModel>
{
    public GameShopView() => AvaloniaXamlLoader.Load(this);

    public GameShopView(GameShopViewModel gameShopViewModel) : this()
    {
        ViewModel = gameShopViewModel;

        this.WhenActivated(async d =>
        {
            await ViewModel.LoadAsync();
        });
    }
}