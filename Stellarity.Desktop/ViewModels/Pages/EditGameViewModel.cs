using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
using Stellarity.Domain.Models;
using Stellarity.Domain.Services;
using Stellarity.Domain.Validation;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class EditGameViewModel : ViewModelBase
{
    private readonly Game _game;

    private readonly MainViewModel _windowOwner;
    private readonly IDialogService _dialogService;

    private readonly NavigationPublisher _navigator;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddCommand))]
    private string _title = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddCommand))]
    private string _description = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddCommand))]
    private string _developer = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddCommand))]
    private decimal _cost;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddCommand))]
    private bool _free;

    [ObservableProperty]
    private Bitmap? _cover;

    public EditGameViewModel(Game game, IDialogService dialogService, NavigationPublisher navigator)
    {
        _game = game;
        _dialogService = dialogService;
        _navigator = navigator;

        _windowOwner = DiContainingService.Kernel.Get<MainViewModel>();
        Validator = GetValidator();
    }

    public bool CanEdit => Validator is { IsValid: true };

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

    [RelayCommand(CanExecute = nameof(CanEdit))]
    private async Task AddAsync()
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
            .Must(async (title, token) => await GameValidation.NotExistsWithTitleAsync(title, token))
            .WithMessage("Game with such already exists");

        builder.RuleFor(vm => vm.Description)
            .NotEmpty();

        builder.RuleFor(vm => vm.Developer)
            .NotEmpty()
            .MaxLength(30);

        builder.RuleFor(vm => vm.Cost)
            .Between(0, 5000);

        return builder.Build(this);
    }
}