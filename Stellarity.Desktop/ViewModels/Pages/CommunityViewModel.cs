using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.ViewModels.Wraps;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class CommunityViewModel : ViewModelBase, IAsyncLoader
{
    private const int AccountsByPage = 5;

    [ObservableProperty]
    private int _pageCount;

    [ObservableProperty]
    private int _currentPageNumber;

    public CommunityViewModel()
    {
        PageCount = AccountEntity.GetAccountsCount(AccountsByPage);
        CurrentPageNumber = 0;
    }

    public ObservableCollection<AccountRowViewModel> Users { get; } = new();

    async partial void OnCurrentPageNumberChanged(int value)
    {
        await LoadUsersForPage();
    }

    public async Task LoadAsync()
    {
        PageCount = AccountEntity.GetAccountsCount(AccountsByPage);
        await LoadUsersForPage();
    }

    private async Task LoadUsersForPage()
    {
        Users.Clear();
        var users = await Account.GetAccountsAsync(CurrentPageNumber, AccountsByPage);
        foreach (var account in users)
            Users.Add(new AccountRowViewModel(account));
    }
}