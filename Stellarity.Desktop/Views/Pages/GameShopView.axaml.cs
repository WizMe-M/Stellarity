using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class GameShopView : ReactiveUserControl<GameShopViewModel>, IContentPage
{
    public GameShopView()
    {
        AvaloniaXamlLoader.Load(this);
        this.WhenActivated(async d => { await ViewModel!.LoadAsync(); });
    }


    public async Task InitializeViewModelAsync(GameShopViewModel viewModel)
    {
        ViewModel = viewModel;
        await viewModel.LoadAsync();
    }
}