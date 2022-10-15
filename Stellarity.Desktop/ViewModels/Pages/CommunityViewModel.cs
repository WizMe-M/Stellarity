using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.ViewModels.Wraps;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class CommunityViewModel : ViewModelBase, IAsyncLoader
{
    private const int AccountsByPage = 10;

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

    public async Task LoadAsync()
    {
        Users.Clear();
        PageCount = AccountEntity.GetAccountsCount(AccountsByPage);
        var users = await Account.GetAccountsAsync(CurrentPageNumber, AccountsByPage);
        foreach (var account in users)
            Users.Add(new AccountRowViewModel(account));
    }
}