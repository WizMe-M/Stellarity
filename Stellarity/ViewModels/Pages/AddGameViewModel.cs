using System.Drawing;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveValidation;
using Stellarity.Basic.CommunityMvvm;

namespace Stellarity.ViewModels.Pages;

public partial class AddGameViewModel : ViewModelBase
{
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private string _developer = string.Empty;
    [ObservableProperty] private double _cost;
    [ObservableProperty] private bool _free = true;
    [ObservableProperty] private Bitmap? _cover;

    public AddGameViewModel()
    {
        Validator = GetValidator();
    }

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<AddGameViewModel>();

        return builder.Build(this);
    }

    [RelayCommand]
    private async Task SetCover()
    {
        await Task.Delay(3000);
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        await Task.Delay(3000);
    }
}