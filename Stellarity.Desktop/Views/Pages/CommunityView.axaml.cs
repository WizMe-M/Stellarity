using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.Desktop.Navigation.Event;
using Stellarity.Desktop.ViewModels.Pages;

namespace Stellarity.Desktop.Views.Pages;

public partial class CommunityView : ReactiveUserControl<CommunityViewModel>, IContentPage
{
    public CommunityView()
    {
        AvaloniaXamlLoader.Load(this);
        this.WhenActivated(async d => await ViewModel!.LoadAsync());
    }
}