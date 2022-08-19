using System.Collections.Specialized;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.Basic;

namespace Stellarity.UserControls;

[TemplatePart("PART_RoutedViewHost", typeof(RoutedViewHost))]
public class HamburgerMenu : TabControl, IScreen
{
    private SplitView? _splitView;

    /// <summary>
    /// Important routing thing
    /// </summary>
    public RoutingState Router { get; } = new();

    protected override void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        base.ItemsCollectionChanged(sender, e);
        
        var avaloniaList = (AvaloniaList<object>)sender;
        if(avaloniaList[^1] is not HamburgerItem hamburgerItem) return;

        hamburgerItem.SelectedClick += OnSelectedClick;
        hamburgerItem.UnselectedClick += OnUnselectedClick;
    }

    public static readonly StyledProperty<IBrush?> PaneBackgroundProperty =
        SplitView.PaneBackgroundProperty.AddOwner<HamburgerMenu>();

    public IBrush? PaneBackground
    {
        get => GetValue(PaneBackgroundProperty);
        set => SetValue(PaneBackgroundProperty, value);
    }

    public static readonly StyledProperty<IBrush?> ContentBackgroundProperty =
        AvaloniaProperty.Register<HamburgerMenu, IBrush?>(nameof(ContentBackground));

    public IBrush? ContentBackground
    {
        get => GetValue(ContentBackgroundProperty);
        set => SetValue(ContentBackgroundProperty, value);
    }

    public static readonly StyledProperty<int> ExpandedModeThresholdWidthProperty =
        AvaloniaProperty.Register<HamburgerMenu, int>(nameof(ExpandedModeThresholdWidth), 1008);

    public int ExpandedModeThresholdWidth
    {
        get => GetValue(ExpandedModeThresholdWidthProperty);
        set => SetValue(ExpandedModeThresholdWidthProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _splitView = e.NameScope.Find<SplitView>("PART_NavigationPane");
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == BoundsProperty && _splitView is not null)
        {
            var oldBounds = change.OldValue.GetValueOrDefault<Rect>();
            var newBounds = change.NewValue.GetValueOrDefault<Rect>();
            EnsureSplitViewMode(oldBounds, newBounds);
        }
    }

    private void EnsureSplitViewMode(Rect oldBounds, Rect newBounds)
    {
        if (_splitView is null) return;
        
        var threshold = ExpandedModeThresholdWidth;

        if (newBounds.Width >= threshold && oldBounds.Width < threshold)
        {
            _splitView.DisplayMode = SplitViewDisplayMode.Inline;
            _splitView.IsPaneOpen = true;
        }
        else if (newBounds.Width < threshold && oldBounds.Width >= threshold)
        {
            _splitView.DisplayMode = SplitViewDisplayMode.Overlay;
            _splitView.IsPaneOpen = false;
        }
    }

    private void OnSelectedClick(object? sender, RoutedEventArgs routedEventArgs)
    {
        if(sender is not HamburgerItem item) return;
        var vm = item.Content as PageViewModel;
    }

    private void OnUnselectedClick(object? sender, RoutedEventArgs routedEventArgs)
    {
        // clear stack (!) and open item content
    }
}