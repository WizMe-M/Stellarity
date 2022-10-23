using Ninject.Modules;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Email;
using Stellarity.Domain.Import;
using Stellarity.Domain.Services;
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

        Kernel.Bind<MailingService>().ToSelf().InSingletonScope();
        Kernel.Bind<GameChequeSenderService>().ToSelf().InSingletonScope();

        Kernel.Bind<KeyImportService>().ToSelf().InSingletonScope();
    }

    private static async void InitializeOnActivation(AccountingService service) => await service.InitializeAsync();
}