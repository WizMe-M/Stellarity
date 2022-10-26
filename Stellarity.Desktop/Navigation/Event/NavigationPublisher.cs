namespace Stellarity.Desktop.Navigation.Event;

public class NavigationPublisher
{
    public delegate void NavigatedEventHandler(object sender, NavigatedEventArgs args);

    public event NavigatedEventHandler? Navigated;

    public void RaiseNavigated(object sender, NavigatedEventArgs args) => Navigated?.Invoke(sender, args);
}