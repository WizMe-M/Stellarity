using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.ViewModels.Pages;

namespace Stellarity.Views.Pages;

public partial class MyProfileView : ReactiveUserControl<MyProfileViewModel>
{
    public MyProfileView()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public MyProfileView(MyProfileViewModel viewModel) : this()
    {
        ViewModel = viewModel;
        this.WhenActivated(async d => { await ViewModel!.LoadAsync(); });
    }
}