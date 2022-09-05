using Avalonia.Interactivity;

namespace Stellarity.TemplatedControls.Navigation;

/// <summary>
/// Holds the details of the <see cref=".TemplateApplied"/> event.
/// </summary>
public class ChangeContentRequestReceivedEventArgs : RoutedEventArgs
{
    /// <summary>
    /// Represents a ContentPage to display
    /// <remarks>It could be a view or a viewmodel</remarks>
    /// </summary>
    public IContentPage? ContentPage { get; }

    /// <summary>
    /// Represents a strategy how to interact with <see cref="NavigationView"/>'s <see cref="NavigationView._pagesStack" />
    /// </summary>
    public ChangeContentStrategy ChangeContentStrategy { get; }

    /// <summary>
    /// Creates new args
    /// </summary>
    /// <param name="contentPage">Content to display</param>
    /// <param name="strategy">Strategy how to interact with <see cref="NavigationView"/> <see cref="NavigationView._pagesStack"/></param>
    /// <param name="routedEvent"><see cref="NavigationViewItem.ChangeContentRequestReceived"/> event</param>
    /// <remarks>Is hidden to prevent incorrect strategies usage. Use static factory-methods instead</remarks>
    private ChangeContentRequestReceivedEventArgs(IContentPage contentPage, ChangeContentStrategy strategy,
        RoutedEvent routedEvent)
        : base(routedEvent)
    {
        ContentPage = contentPage;
        ChangeContentStrategy = strategy;
    }

    /// <summary>
    /// Creates new args
    /// </summary>
    /// <param name="strategy">Strategy how to interact with <see cref="NavigationView"/> <see cref="NavigationView._pagesStack"/></param>
    /// <param name="routedEvent"><see cref="NavigationViewItem.ChangeContentRequestReceived"/> event</param>
    /// <remarks>Is hidden to prevent incorrect strategies usage. Use static factory-methods instead</remarks>
    private ChangeContentRequestReceivedEventArgs(ChangeContentStrategy strategy, RoutedEvent routedEvent)
        : base(routedEvent)
    {
        ChangeContentStrategy = strategy;
    }

    /// <summary>
    /// Pushes new <see cref="IContentPage"/>
    /// </summary>
    /// <param name="contentPage">Content to display</param>
    /// <param name="routedEvent"><see cref="NavigationViewItem.ChangeContentRequestReceived"/> event</param>
    /// <returns>Args for <see cref="NavigationViewItem.ChangeContentRequestReceived"/> event</returns>
    public static ChangeContentRequestReceivedEventArgs Push(IContentPage contentPage, RoutedEvent routedEvent) =>
        new(contentPage, ChangeContentStrategy.Push, routedEvent);

    /// <summary>
    /// Clears current stack and pushes new <see cref="IContentPage"/>
    /// </summary>
    /// <param name="contentPage">Content to display</param>
    /// <param name="routedEvent"><see cref="NavigationViewItem.ChangeContentRequestReceived"/> event</param>
    /// <returns>Args for <see cref="NavigationViewItem.ChangeContentRequestReceived"/> event</returns>
    public static ChangeContentRequestReceivedEventArgs
        ClearAndPush(IContentPage contentPage, RoutedEvent routedEvent) =>
        new(contentPage, ChangeContentStrategy.ClearAndPush, routedEvent);

    /// <summary>
    /// Tries to remove last opened <see cref="IContentPage"/>
    /// </summary>
    /// <param name="routedEvent"><see cref="NavigationViewItem.ChangeContentRequestReceived"/> event</param>
    /// <returns>Args for <see cref="NavigationViewItem.ChangeContentRequestReceived"/> event</returns>
    public static ChangeContentRequestReceivedEventArgs Pop(RoutedEvent routedEvent) =>
        new(ChangeContentStrategy.Pop, routedEvent);
}