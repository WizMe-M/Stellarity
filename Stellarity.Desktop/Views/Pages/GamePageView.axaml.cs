using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Domain.Models;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class GamePageView : ReactiveUserControl<GamePageViewModel>, IContentPage
{
    public GamePageView() => AvaloniaXamlLoader.Load(this);

    public GamePageView(Game game) : this()
    {
        ViewModel = new GamePageViewModel(game);
        this.WhenActivated(async d => await ViewModel.UpdateGameInfoAsync());
    }
}