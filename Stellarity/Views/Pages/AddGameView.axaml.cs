using Avalonia.Markup.Xaml;
using Stellarity.Basic;
using Stellarity.ViewModels.Pages;

namespace Stellarity.Views.Pages;

public partial class AddGameView : ExtUserControl<AddGameViewModel>
{
    public AddGameView()
    {
        DataContext = null;
        AvaloniaXamlLoader.Load(this);
    }
}