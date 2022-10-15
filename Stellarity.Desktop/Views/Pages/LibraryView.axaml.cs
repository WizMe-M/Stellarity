using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class LibraryView : ReactiveUserControl<LibraryViewModel>, IContentPage
{
    public LibraryView() => AvaloniaXamlLoader.Load(this);

    public LibraryView(NavigationPublisher navigator) : this()
    {
        ViewModel = new LibraryViewModel(navigator);
        this.WhenActivated(async d => await ViewModel.LoadAsync());
    }
}