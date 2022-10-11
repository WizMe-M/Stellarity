using Stellarity.Navigation.Event;

namespace Stellarity.Navigation.Stack;

public class NavigationStack
{
    private readonly Stack<IContentPage> _navStack;

    public NavigationStack(IContentPage root) =>
        _navStack = new Stack<IContentPage>(new[] { root });

    public IContentPage Current => _navStack.Peek();

    public void HandleNavigation(NavigatedEventArgs e)
    {
        switch (e.NavigationStrategy)
        {
            case NavigationStrategy.Pop:
                switch (_navStack.Count)
                {
                    case > 1:
                        _navStack.Pop();
                        break;
                    case 1:
                        throw new NavigationStackException(
                            $"Can't pop last ContentPage, use {NavigationStrategy.ReplaceLast} or {NavigationStrategy.ClearAndPush} instead");
                    default:
                        throw new NavigationStackException("ContentPage stack is empty");
                }

                break;
            case NavigationStrategy.Push:
                _navStack.Push(e.ContentPage!);
                break;
            case NavigationStrategy.ReplaceLast:
                _navStack.Pop();
                _navStack.Push(e.ContentPage!);
                break;
            case NavigationStrategy.ClearAndPush:
                _navStack.Clear();
                _navStack.Push(e.ContentPage!);
                break;
            default:
                throw new NavigationStackException(
                    $"For some reason {nameof(e.NavigationStrategy)} was outside bounds");
        }
    }
}