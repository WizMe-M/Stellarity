using System;

namespace Stellarity.Desktop.Navigation.Event;

public class NavigatedEventArgs : EventArgs
{
    private NavigatedEventArgs(NavigationStrategy navigationStrategy, IContentPage? contentPage = null)
    {
        NavigationStrategy = navigationStrategy;
        ContentPage = contentPage;
    }

    public NavigationStrategy NavigationStrategy { get; }
    public IContentPage? ContentPage { get; }

    #region Factory

    public static NavigatedEventArgs Pop() => new(NavigationStrategy.Pop);
    public static NavigatedEventArgs Push(IContentPage page) => new(NavigationStrategy.Push, page);
    public static NavigatedEventArgs ReplaceLast(IContentPage page) => new(NavigationStrategy.ReplaceLast, page);
    public static NavigatedEventArgs ClearAndPush(IContentPage page) => new(NavigationStrategy.ClearAndPush, page);

    #endregion
}