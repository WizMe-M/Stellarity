namespace Stellarity.Navigation.Event;

public enum NavigationStrategy
{
    /// <summary>
    /// Remove last content page from navigation stack
    /// </summary>
    Pop = 1,

    /// <summary>
    /// Add content page to the navigation stack 
    /// </summary>
    Push = 2,

    /// <summary>
    /// Replace last content page from navigation stack with new content page
    /// </summary>
    ReplaceLast = 3,

    /// <summary>
    /// Clear all navigation stack and add new content page
    /// </summary>
    ClearAndPush = 4
}