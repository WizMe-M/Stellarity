using System;

namespace Stellarity.Desktop.Navigation.Stack;

/// <summary>
/// Represents exception raised in navigation stack
/// </summary>
public class NavigationStackException : Exception
{
    public NavigationStackException(string message) => Message = message;
    public override string Message { get; }
}