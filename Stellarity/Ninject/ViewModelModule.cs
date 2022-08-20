using System;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Stellarity.Basic;

namespace Stellarity.Ninject;

public class ViewModelModule : NinjectModule
{
    public override void Load()
    {
        if (Kernel is null) throw new InvalidOperationException("Kernel was null");

        Kernel.Bind(c => c.FromThisAssembly()
            .Select(typeof(IViewModelBase).IsAssignableFrom)
            .BindToSelf());
    }
}