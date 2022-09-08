using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Stellarity.Basic;
using Stellarity.TemplatedControls.Navigation;
using Stellarity.ViewModels.Pages;

namespace Stellarity.Views.Pages;

public partial class MyProfileView : ExtUserControl<MyProfileViewModel>, IContentPage
{
    public MyProfileView()
    {
        DataContext = null;
        AvaloniaXamlLoader.Load(this);
    }

    public override async Task InitializeViewModelAsync(MyProfileViewModel viewModel)
    {
        await base.InitializeViewModelAsync(viewModel);
        await viewModel.LoadAsync();
    }
}