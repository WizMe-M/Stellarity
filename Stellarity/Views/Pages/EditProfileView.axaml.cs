using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Stellarity.Basic;
using Stellarity.ViewModels.Pages;

namespace Stellarity.Views.Pages;

public partial class EditProfileView : ExtUserControl<EditProfileViewModel>
{
    public EditProfileView() => AvaloniaXamlLoader.Load(this);

    public override async Task InitializeViewModelAsync(EditProfileViewModel viewModel)
    {
        await base.InitializeViewModelAsync(viewModel);
        await viewModel.LoadAsync();
    }
}