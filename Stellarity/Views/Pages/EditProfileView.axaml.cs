using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.ViewModels.Pages;

namespace Stellarity.Views.Pages;

public partial class EditProfileView : ReactiveUserControl<EditProfileViewModel>
{
    public EditProfileView()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public EditProfileView(EditProfileViewModel viewModel) : this()
    {
        ViewModel = viewModel;
        this.WhenActivated(async d =>
        {
            await ViewModel!.LoadAsync();
        });
    }
}