using System;
using Ninject.Modules;

namespace Stellarity.Ninject;

public class ViewModelModule : NinjectModule
{
    public override void Load()
    {
        if (Kernel is null) throw new InvalidOperationException("Kernel was null");

        // cannot DI ViewModels (because of it is runtime obj)
        // Kernel.Bind(c => c.FromThisAssembly()
        //     .Select(typeof(IViewModelBase).IsAssignableFrom)
        //     .BindToSelf());
    }
}