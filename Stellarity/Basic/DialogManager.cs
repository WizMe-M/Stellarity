using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Microsoft.Extensions.Logging;
using Stellarity.Basic.CommunityMvvm;

namespace Stellarity.Basic;

public class DialogManager : DialogManagerBase<Window>
{
    public DialogManager(IViewLocator viewLocator, IDialogFactory dialogFactory, 
        ILogger<DialogManagerBase<Window>>? logger = null) 
        : base(viewLocator, dialogFactory, logger)
    {
    }

    protected override IWindow CreateWrapper(Window window) => window.AsWrapper();

    protected override void Dispatch(Action action)
    {
    }

    private static IEnumerable<Window> Windows
    {
        get
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime applicationLifetime)
                return applicationLifetime.Windows;
            return ArraySegment<Window>.Empty;
        }
    }
    
    

    protected override Task<T> DispatchAsync<T>(Func<T> action) => Task.FromResult(action());

    public override IWindow? FindWindowByViewModel(INotifyPropertyChanged viewModel)
    {
        var view = Windows.FirstOrDefault(x => 
            (x.DataContext as ViewModelBase)!.Id == (viewModel as ViewModelBase)!.Id); 
        return view.AsWrapper();
    }
}