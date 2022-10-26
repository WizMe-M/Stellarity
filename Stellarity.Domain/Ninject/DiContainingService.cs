using Ninject;
using Ninject.Modules;

namespace Stellarity.Domain.Ninject;

public class DiContainingService
{
    public static StandardKernel Kernel { get; private set; } = null!;

    private DiContainingService(params INinjectModule[] modules)
    {
        var settings = new NinjectSettings {AllowNullInjection = true };
        Kernel = new StandardKernel(settings, modules);
    }

    public static void Initialize(params INinjectModule[] modules)
    {
        var _ = new DiContainingService(modules);
    }
}