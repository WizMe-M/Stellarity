using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class MyProfileView : ExtUserControl<MyProfileViewModel>, IContentPage
{
    public MyProfileView() => AvaloniaXamlLoader.Load(this);

    public override async Task InitializeViewModelAsync(MyProfileViewModel viewModel)
    {
        await base.InitializeViewModelAsync(viewModel);
        await viewModel.LoadAsync();
    }
}