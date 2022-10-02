using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Stellarity.Avalonia.ViewModel;

namespace Stellarity.Desktop.Basic;

public class DialogManager : HanumanInstitute.MvvmDialogs.Avalonia.DialogManager
{
    private static IEnumerable<Window> Windows
    {
        get
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime applicationLifetime)
                return applicationLifetime.Windows;
            return ArraySegment<Window>.Empty;
        }
    }

    public override IView? FindWindowByViewModel(INotifyPropertyChanged viewModel)
    {
        var view = Windows.FirstOrDefault(x =>
        {
            var vm1 = x.DataContext as ViewModelBase;
            var vm2 = viewModel as ViewModelBase;
            var result = vm1!.Id == vm2!.Id; 
            return result;
        }); 
        return view.AsWrapper();
    }
}