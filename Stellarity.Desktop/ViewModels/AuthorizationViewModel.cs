using System.Reactive;
using System.Threading.Tasks;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Basic;
using Stellarity.Services;
using Stellarity.Services.Accounting;

namespace Stellarity.Desktop.ViewModels;

public class AuthorizationViewModel : ViewModelBase
{
    private readonly IDialogService _windowService = null!;

    public AuthorizationViewModel()
    {
        RememberUser = true;

        LogIn = ReactiveCommand.CreateFromTask(async () =>
        {
            // TODO: authorization logic
            var accountingService = DiContainingService.Kernel.Get<AccountingService>();
            accountingService.AuthorizedAccount = Account.GetFirst();
            await Task.Delay(1000);

            var vm = _windowService.CreateViewModel<MainViewModel>();
            _windowService.Show(this, vm);
            _windowService.Close(this);
        });
    }

    // ReSharper disable once UnusedMember.Global
    public AuthorizationViewModel(IDialogService windowService) : this()
    {
        _windowService = windowService;
    }

    [Reactive]
    public string Email { get; set; } = "";

    [Reactive]
    public string Password { get; set; } = "";

    [Reactive]
    public bool RememberUser { get; set; }

    public ReactiveCommand<Unit, Unit> LogIn { get; }
}