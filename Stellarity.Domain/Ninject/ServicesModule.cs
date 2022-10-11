using Ninject.Modules;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Services.Cache;

namespace Stellarity.Domain.Ninject;

public class ServicesModule : NinjectModule
{
    public override void Load()
    {
        if (Kernel is null) throw new InvalidOperationException("Kernel was null");

        Kernel.Bind<Cacher>().ToSelf().InSingletonScope();
        Kernel.Bind<AccountingService>().ToSelf().InSingletonScope()
            .OnActivation(InitializeOnActivation);
    }

    private static async void InitializeOnActivation(AccountingService service) => await service.InitializeAsync();
}