using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using HanumanInstitute.MvvmDialogs.Avalonia;
using ReactiveUI;

namespace Stellarity.Desktop.Basic;

internal class ViewLocator : ViewLocatorBase
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
}