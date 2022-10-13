using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class GameShopView : ExtUserControl<GameShopViewModel>, IContentPage
{
    public GameShopView() => AvaloniaXamlLoader.Load(this);

    public override async Task InitializeViewModelAsync(GameShopViewModel viewModel)
    {
        await base.InitializeViewModelAsync(viewModel);
        await viewModel.LoadAsync();
    }
}