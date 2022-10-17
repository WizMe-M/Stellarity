using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Ninject;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Views;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Services;

namespace Stellarity.Desktop.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [RelayCommand]
    private async Task ExitAsync()
    {
        var dialogService = DiContainingService.Kernel.Get<IDialogService>();
        var accountingService = DiContainingService.Kernel.Get<AccountingService>();

        var wantLogOut = await dialogService.ShowMessageBoxAsync(this,
            "Do you really want to log out of your account?", "Confirm log out",
            MessageBoxButton.YesNo, MessageBoxImage.Information, false);

        if (wantLogOut == true)
        {
            accountingService.LogOut();
            var authorizationViewModel = dialogService.CreateViewModel<AuthorizationViewModel>();
            dialogService.Show<AuthorizationView>(this, authorizationViewModel);
            dialogService.Close(this);
        }
    }
}