using ReactiveUI;
using Stellarity.UserControls;

namespace Stellarity.Basic;

public abstract class PageViewModel : ValidatableViewModelBase, IRoutableViewModel
{
    protected PageViewModel(IScreen screen)
    {
        HostScreen = screen;
    }

    public IScreen HostScreen { get; }
    public string? UrlPathSegment { get; }
}