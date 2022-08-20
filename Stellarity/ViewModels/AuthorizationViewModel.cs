using System.Reactive;
using System.Threading.Tasks;
using HanumanInstitute.MvvmDialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Stellarity.ViewModels;

public class AuthorizationViewModel : Basic.ReactiveViewModelBase
{
    private IDialogService _windowService = null!;

    public AuthorizationViewModel()
    {
        RememberUser = true;

        LogIn = ReactiveCommand.CreateFromTask(async () =>
        {
            // TODO: authorization logic
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