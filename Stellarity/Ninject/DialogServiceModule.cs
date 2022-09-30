using System;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Ninject;
using Ninject.Modules;
using Stellarity.Basic;
using DialogManager = Stellarity.Basic.DialogManager;

namespace Stellarity.Ninject;

public class DialogServiceModule : NinjectModule
{
    public override void Load()
    {
        if (Kernel is null) throw new InvalidOperationException("Kernel was null");

        Kernel.Bind<IViewLocator>().To<DialogViewLocator>();
        Kernel.Bind<ReactiveUI.IViewLocator>().To<ViewLocator>();
        Kernel.Bind<IDialogFactory>().To<DialogFactory>()
            .WhenInjectedInto<DialogManager>();
        Kernel.Bind<IDialogManager>().To<DialogManager>();
        Kernel.Bind<IDialogService>().To<DialogService>()
            .WithConstructorArgument("viewModelFactory",
                (Func<Type, object>)(x => Kernel.Get(x)));
    }
}