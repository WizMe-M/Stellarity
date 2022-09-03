using System;
using Ninject.Modules;
using Stellarity.Services.Accounting;
using Stellarity.Services.Cache;

namespace Stellarity.Ninject;

public class ServicesModule : NinjectModule
{
    public override void Load()
    {
        if (Kernel is null) throw new InvalidOperationException("Kernel was null");

        Kernel.Bind<CachingService>().ToSelf().InSingletonScope();
        Kernel.Bind<AccountingService>().ToSelf().InSingletonScope()
            .OnActivation(async service => await service.InitializeAsync());
    }
}