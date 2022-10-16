using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class RegisterUserView : ReactiveUserControl<RegisterUserViewModel>, IContentPage
{
    public RegisterUserView() => AvaloniaXamlLoader.Load(this);
}