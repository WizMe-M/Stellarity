using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Ninject;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.ViewModels.Wraps;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.ViewModels.Pages;

public class LibraryViewModel : ViewModelBase, IAsyncImageLoader
{
    private readonly NavigationPublisher _navigator;
    private readonly Account _owner;
    
    public LibraryViewModel(NavigationPublisher navigator)
    {
        _navigator = navigator;
        var accountingService = DiContainingService.Kernel.Get<AccountingService>();
        _owner = accountingService.AuthorizedUser!;
    }

    public ObservableCollection<LibraryGameViewModel> Library { get; } = new();

    public async Task LoadAsync()
    {
        await _owner.RefreshLibraryAsync();
        Library.Clear();
        var games = _owner.Library
            .Select(game => new LibraryGameViewModel(game, _navigator));
        Library.AddRange(games);

        foreach (IAsyncImageLoader game in Library)
            await game.LoadAsync();
    }
}