using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class EditProfileView : ExtUserControl<EditProfileViewModel>, IContentPage
{
    public EditProfileView() => AvaloniaXamlLoader.Load(this);

    public override async Task InitializeViewModelAsync(EditProfileViewModel viewModel)
    {
        await base.InitializeViewModelAsync(viewModel);
        await viewModel.LoadAsync();
    }
}