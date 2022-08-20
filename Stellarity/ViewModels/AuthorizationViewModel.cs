using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Stellarity.ViewModels;

public class AuthorizationViewModel : Basic.ReactiveViewModelBase
{
    public AuthorizationViewModel()
    {
        RememberUser = true;

        LogIn = ReactiveCommand.CreateFromTask(async () =>
        {
            // TODO: authorization logic
            await Task.Delay(1000);
        });
    }

    [Reactive]
    public string Email { get; set; } = "";

    [Reactive]
    public string Password { get; set; } = "";

    [Reactive]
    public bool RememberUser { get; set; }

    public ReactiveCommand<Unit, Unit> LogIn { get; }
}