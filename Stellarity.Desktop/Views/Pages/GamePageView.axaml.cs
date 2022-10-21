using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Domain.Models;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class GamePageView : ReactiveUserControl<GamePageViewModel>, IContentPage
{
    public GamePageView()
    {
        AvaloniaXamlLoader.Load(this);
        this.WhenActivated(async d => await ViewModel!.UpdatePageAsync());
    }

    public GamePageView(Game game, NavigationPublisher navigator) : this()
    {
        ViewModel = new GamePageViewModel(game, navigator);
    }

    public GamePageView(Key gameKey, NavigationPublisher navigator) : this()
    {
        ViewModel = new GamePageViewModel(gameKey, navigator);
    }
}