using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class AddGameView : ReactiveUserControl<AddGameViewModel>, IContentPage
{
    public AddGameView() => AvaloniaXamlLoader.Load(this);
}