using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Stellarity.Basic;
using Stellarity.ViewModels.Pages;

namespace Stellarity.Views.Pages;

public partial class GameShopView : ExtUserControl<GameShopViewModel>
{
    public GameShopView()
    {
        DataContext = null;
        AvaloniaXamlLoader.Load(this);
    }

    public override async Task InitializeViewModelAsync(GameShopViewModel viewModel)
    {
        await base.InitializeViewModelAsync(viewModel);
        await viewModel.LoadAsync();
    }
}