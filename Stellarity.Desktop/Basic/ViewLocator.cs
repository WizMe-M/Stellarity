using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using HanumanInstitute.MvvmDialogs.Avalonia;
using ReactiveUI;
using Stellarity.Avalonia.ViewModel;

namespace Stellarity.Desktop.Basic;

internal class ViewLocator : ViewLocatorBase, IViewLocator
{
    public override IControl Build(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = $"Not Found: {name}" };
    }

    public override bool Match(object? data) => data is IViewModelBase;

    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        if (viewModel is null) return null;

        var name = viewModel.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (IViewFor)Activator.CreateInstance(type)!;
        }

        return null;
    }
}