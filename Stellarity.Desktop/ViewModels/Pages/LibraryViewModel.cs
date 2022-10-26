using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Ninject;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Navigation.Event;
using Stellarity.Desktop.ViewModels.Wraps;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;
using Stellarity.Domain.Ninject;

namespace Stellarity.Desktop.ViewModels.Pages;

public class LibraryViewModel : ViewModelBase, IAsyncLoader
{
    private readonly NavigationPublisher _navigator;
    private readonly Account _owner;

    public LibraryViewModel(NavigationPublisher navigator)
    {
        _navigator = navigator;
        var accountingService = DiContainingService.Kernel.Get<AccountingService>();
        _owner = accountingService.AuthorizedUser!;
    }

    public ObservableCollection<PurchasedGameViewModel> Library { get; } = new();

    public async Task LoadAsync()
    {
        var keys = await _owner.RefreshLibraryAsync();
        Library.Clear();
        await foreach (var libraryItem in KeysToLibrary(keys))
            Library.Add(libraryItem);
    }

    private async IAsyncEnumerable<PurchasedGameViewModel> KeysToLibrary(IEnumerable<Key> keys)
    {
        foreach (var key in keys)
        {
            var game = new PurchasedGameViewModel(key, _navigator);
            await game.LoadAsync();
            yield return game;
        }
    }
}