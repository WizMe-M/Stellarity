using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Stellarity.TemplatedControls.Navigation;

[TemplatePart("PART_SplitView", typeof(SplitView))]
public class NavigationView : TabControl
{
    private readonly Stack<IContentPage> _pagesStack = new();
    private SplitView _rootSplitView = null!;

    public static readonly StyledProperty<double> CompactPaneLengthProperty =
        AvaloniaProperty.Register<NavigationView, double>(nameof(CompactPaneLength));

    public double CompactPaneLength
    {
        get => GetValue(CompactPaneLengthProperty);
        set => SetValue(CompactPaneLengthProperty, value);
    }


    protected override void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        base.ItemsCollectionChanged(sender, e);
        if (e.NewItems is { })
            foreach (var item in e.NewItems)
            {
                if (item is not NavigationViewItem navItem) continue;
                navItem.ChangeContentRequestReceived += OnContentChange;
            }

        if (e.OldItems is { })
            foreach (var item in e.OldItems)
            {
                if (item is not NavigationViewItem navItem) continue;
                navItem.ChangeContentRequestReceived -= OnContentChange;
            }

        Debug.WriteLine("Subscriptions were done");
    }

    private void OnContentChange(object? sender, ChangeContentRequestReceivedEventArgs e)
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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _rootSplitView = e.NameScope.Get<SplitView>("PART_SplitView");
    }
}