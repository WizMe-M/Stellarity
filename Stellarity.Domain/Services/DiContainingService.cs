using Ninject;
using Ninject.Modules;
using Stellarity.Domain.Ninject;

namespace Stellarity.Domain.Services;

public class DiContainingService
{
    public static StandardKernel Kernel { get; private set; } = null!;

    public DiContainingService()
    {
        var settings = new NinjectSettings { AllowNullInjection = true };
        Kernel = new StandardKernel(settings, new ServicesModule());
    }

    public DiContainingService(params NinjectModule[] additionalModules)
    {
        var settings = new NinjectSettings
        {
            AllowNullInjection = true,
        };
        var modules = additionalModules.Concat(new[] { new ServicesModule() })
            .Cast<INinjectModule>().ToArray();
        Kernel = new StandardKernel(settings, modules);
    }

    public static void Initialize(params NinjectModule[] additionalModules)
    {
        var _ = new DiContainingService(additionalModules);
    }
}