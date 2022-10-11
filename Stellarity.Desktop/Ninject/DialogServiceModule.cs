using System;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Ninject;
using Ninject.Modules;

namespace Stellarity.Desktop.Ninject;

public class DialogServiceModule : NinjectModule
{
    public override void Load()
    {
        if (Kernel is null) throw new InvalidOperationException("Kernel was null");

        var dialogService = new DialogService(
            dialogManager: new DialogManager(
                viewLocator: new ViewLocatorBase(),
                dialogFactory: new DialogFactory().AddMessageBox()),
            viewModelFactory: x => Kernel.Get(x));

        Kernel.Bind<IDialogService>()
            .ToConstant(dialogService)
            .InSingletonScope();
    }
}