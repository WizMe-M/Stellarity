using Avalonia.Markup.Xaml;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class AddGameView : ExtUserControl<AddGameViewModel>, IContentPage
{
    public AddGameView()
    {
        DataContext = null;
        AvaloniaXamlLoader.Load(this);
    }
}