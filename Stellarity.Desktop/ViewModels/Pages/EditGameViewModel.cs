using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Ninject;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Desktop.Basic;
using Stellarity.Desktop.Extensions;
using Stellarity.Desktop.Navigation.Event;
using Stellarity.Domain.Models;
using Stellarity.Domain.Ninject;
using Stellarity.Domain.Validation;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class EditGameViewModel : ViewModelBase, IAsyncLoader
{
    private readonly Game _game;
    private readonly MainViewModel _windowOwner;
    private readonly IDialogService _dialogService;
    private readonly NavigationPublisher _navigator;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private string _title = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private string _description = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private string _developer = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private decimal _cost;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    private bool _free;

    [ObservableProperty] private Bitmap? _cover;

    private string _currentTitle;

    public EditGameViewModel(Game game, NavigationPublisher navigator)
    {
        _game = game;
        _navigator = navigator;
        _currentTitle = game.Title;
        Title = _game.Title;
        Description = _game.Description;
        Developer = _game.Developer;
        Cost = _game.Cost;
        Free = Cost == 0;

        _dialogService = DiContainingService.Kernel.Get<IDialogService>();
        _windowOwner = DiContainingService.Kernel.Get<MainViewModel>();
        Validator = GetValidator();
    }

    public bool CanSaveChanges => Validator is { IsValid: true };

    [RelayCommand]
    private async Task SetCover()
    {
        var settings = new OpenFileDialogSettings
        {
            Title = "Выберите изображение для обложки игры",
            InitialDirectory = Directory.GetDirectoryRoot(Assembly.GetExecutingAssembly().Location),
            Filters = new List<FileFilter>(new[]
            {
                new FileFilter("Изображения", new[] { "jpg", "jpeg", "png" })
            })
        };

        var imgPath = await _dialogService.ShowOpenFileDialogAsync(_windowOwner, settings);
        if (imgPath is { })
        {
            Cover = new Bitmap(imgPath);
        }
    }

    [RelayCommand(CanExecute = nameof(CanSaveChanges))]
    private async Task SaveChangesAsync()
    {
        if (Free) Cost = 0;
        Title = Title.Trim();
        Description = Description.Trim();
        Developer = Developer.Trim();

        var coverData = Cover.FromBitmap();
        await _game.EditAsync(Title, Description, Developer, Cost, coverData);
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Pop());
    }

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<EditGameViewModel>();

        builder.RuleFor(vm => vm.Title)
            .NotEmpty()
            .MaxLength(25)
            .Must(async (title, token) => await TitleIsDuplicateOtherGame(title, token))
            .WithMessage("Game with such title already exists");

        builder.RuleFor(vm => vm.Description)
            .NotEmpty();

        builder.RuleFor(vm => vm.Developer)
            .NotEmpty()
            .MaxLength(30);

        builder.RuleFor(vm => vm.Cost)
            .Between(0, 5000);

        return builder.Build(this);
    }

    public async Task LoadAsync()
    {
        var bitmap = await _game.GetImageBitmapAsync();
        if (bitmap is { }) Cover = bitmap;
    }

    private async Task<bool> TitleIsDuplicateOtherGame(string title, CancellationToken token)
        => title == _currentTitle || await GameValidation.NotExistsWithTitleAsync(title, token);
}