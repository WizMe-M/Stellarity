using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Stellarity.Desktop.Navigation.Event;
using Stellarity.Desktop.Navigation.Stack;

namespace Stellarity.Desktop.TemplatedControls.Navigation;

[TemplatePart("PART_SplitView", typeof(SplitView))]
public class NavigationView : TabControl
{
    private readonly NavigationStack _navigationStack;
    private ICommand _exitCommand = null!;

    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<NavigationView, string>(nameof(Header));

    public static readonly StyledProperty<IContentPage> ContentPageProperty =
        AvaloniaProperty.Register<NavigationView, IContentPage>(
            nameof(ContentPage));

    public static readonly DirectProperty<NavigationView, ICommand> ExitCommandProperty =
        AvaloniaProperty.RegisterDirect<NavigationView, ICommand>(nameof(ExitCommand),
            o => o.ExitCommand, (o, v) => o.ExitCommand = v);

    public NavigationView()
    {
        _navigationStack = new NavigationStack(new DefaultContent());
        ContentPage = _navigationStack.Current;

        Navigator = new NavigationPublisher();
        Navigator.Navigated += OnNavigated;
    }

    public NavigationPublisher Navigator { get; }

    public ICommand ExitCommand
    {
        get => _exitCommand;
        set => SetAndRaise(ExitCommandProperty, ref _exitCommand, value);
    }

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

    private void OnNavigated(object sender, NavigatedEventArgs args)
    {
        _navigationStack.HandleNavigation(args);
        ContentPage = _navigationStack.Current;
    }
}