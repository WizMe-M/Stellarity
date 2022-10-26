using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Navigation.Event;
using Stellarity.Desktop.ViewModels.Wraps;
using Stellarity.Desktop.Views.Pages;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class CommunityViewModel : ViewModelBase, IAsyncLoader
{
    private readonly NavigationPublisher _navigator;
    private const int AccountsByPage = 5;

    [ObservableProperty]
    private int _pageCount;

    [ObservableProperty]
    private int _currentPageNumber;

    public CommunityViewModel(NavigationPublisher navigator)
    {
        _navigator = navigator;
        var accountingService = DiContainingService.Kernel.Get<AccountingService>();
        User = accountingService.AuthorizedUser!;
        PageCount = AccountEntity.GetAccountsCount(AccountsByPage);
        CurrentPageNumber = 0;
    }

    public Account User { get; }
    public ObservableCollection<AccountRowViewModel> Users { get; } = new();

    [RelayCommand]
    private void NavigateToRegisterUser()
    {
        var dialog = DiContainingService.Kernel.Get<IDialogService>();
        var view = new RegisterUserView { ViewModel = new RegisterUserViewModel(_navigator, dialog) };
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Push(view));
    }

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
        {
            var banRight = !User.IsIdenticalWith(account) && User.CanBanUsers;
            Users.Add(new AccountRowViewModel(account, banRight, _navigator));
        }
    }
}