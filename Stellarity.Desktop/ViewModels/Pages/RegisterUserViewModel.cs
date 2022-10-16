using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class RegisterUserViewModel : ViewModelBase
{
    [ObservableProperty]
    private Roles _selectedRole;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    public RegisterUserViewModel()
    {
        AvailableRoles.AddRange(Enum.GetValues<Roles>());
        SelectedRole = Roles.Player;
    }

    public ObservableCollection<Roles> AvailableRoles { get; } = new();

    [RelayCommand]
    private async Task RegisterAsync()
    {
    }

    [RelayCommand]
    private void Cancel()
    {
    }
}