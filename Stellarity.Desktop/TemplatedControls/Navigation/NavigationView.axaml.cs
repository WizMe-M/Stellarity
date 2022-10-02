using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Stellarity.Navigation.Event;
using Stellarity.Navigation.Stack;

namespace Stellarity.Desktop.TemplatedControls.Navigation;

[TemplatePart("PART_SplitView", typeof(SplitView))]
public class NavigationView : TabControl
{
    public NavigationPublisher Navigator { get; }
    private readonly NavigationStack _navigationStack;

    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<NavigationView, string>(nameof(Header));

    public static readonly StyledProperty<IContentPage> ContentPageProperty = AvaloniaProperty.Register<NavigationView, IContentPage>(
        nameof(ContentPage));

    public IContentPage ContentPage
    {
        get => GetValue(ContentPageProperty);
        set => SetValue(ContentPageProperty, value);
    }
    
    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public NavigationView()
    {
        _navigationStack = new NavigationStack(new DefaultContent());
        ContentPage = _navigationStack.Current;

        Navigator = new NavigationPublisher();
        Navigator.Navigated += OnNavigated;
    }

    private void OnNavigated(object sender, NavigatedEventArgs args)
    {
        _navigationStack.HandleNavigation(args);
        ContentPage = _navigationStack.Current;
    }
}