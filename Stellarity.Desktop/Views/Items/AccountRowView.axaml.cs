using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.Desktop.ViewModels.Wraps;

namespace Stellarity.Desktop.Views.Items;

public partial class AccountRowView : ReactiveUserControl<AccountRowViewModel>
{
    public AccountRowView()
    {
        AvaloniaXamlLoader.Load(this);
        this.WhenActivated(async d => await ViewModel!.LoadAsync());
    }
}