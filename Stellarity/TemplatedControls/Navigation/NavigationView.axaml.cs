using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;

namespace Stellarity.TemplatedControls.Navigation;

[TemplatePart("PART_SplitView", typeof(SplitView))]
public class NavigationView : TabControl
{
    private readonly Stack<IContentPage> _pagesStack = new();

    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<NavigationView, string>(nameof(Header));

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    protected override void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        base.ItemsCollectionChanged(sender, e);
        if (e.NewItems is { })
            foreach (var item in e.NewItems)
            {
                if (item is not NavigationViewItem navItem) continue;
                navItem.ChangeContentRequestReceived += OnContentRequestChange;
            }

        if (e.OldItems is { })
            foreach (var item in e.OldItems)
            {
                if (item is not NavigationViewItem navItem) continue;
                navItem.ChangeContentRequestReceived -= OnContentRequestChange;
            }

        Debug.WriteLine("Subscriptions were done");
    }

    private void OnContentRequestChange(object? sender, ChangeContentRequestReceivedEventArgs e)
    {
        switch (e.ChangeContentStrategy)
        {
            case ChangeContentStrategy.Pop:
                switch (_pagesStack.Count)
                {
                    case > 1:
                        _pagesStack.Pop();
                        break;
                    case 1:
                        throw new InvalidOperationException(
                            $"Can't pop last ContentPage, use {ChangeContentStrategy.ReplaceLast} or {ChangeContentStrategy.ClearAndPush} instead");
                    default:
                        throw new InvalidOperationException("ContentPage stack is empty");
                }

                break;
            case ChangeContentStrategy.Push:
                _pagesStack.Push(e.ContentPage!);
                break;
            case ChangeContentStrategy.ReplaceLast: break;
            case ChangeContentStrategy.ClearAndPush: break;
            default:
                throw new InvalidOperationException($"{nameof(e.ChangeContentStrategy)} was outside bounds");
        }
    }
}