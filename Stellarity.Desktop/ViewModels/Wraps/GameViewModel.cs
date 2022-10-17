using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Extensions;
using Stellarity.Desktop.Views.Pages;
using Stellarity.Domain.Authorization;
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.ViewModels.Wraps;

public partial class GameViewModel : ViewModelBase, IAsyncLoader
{
    private readonly NavigationPublisher _navigator;

    [ObservableProperty]
    private Bitmap? _cover;

    public GameViewModel(Game instance, NavigationPublisher navigator)
    {
        _navigator = navigator;
        Instance = instance;
        _cover = Instance.GetImageBitmapOrDefault();
    }

    public Game Instance { get; }

    [RelayCommand]
    private void DepositOnBalance()
    {
        var dialogService = DiContainingService.Kernel.Get<IDialogService>();
        // dialogService.CreateViewModel<>()
        
    }
    
    [RelayCommand]
    private void OpenGamePage()
    {
        var view = new GamePageView(Instance, _navigator);
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Push(view));
    }

    public async Task LoadAsync()
    {
        var bitmap = await Instance.GetImageBitmapAsync();
        if (bitmap is { }) Cover = bitmap;
    }
}