using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using ReactiveValidation;
using ReactiveValidation.Extensions;
using Stellarity.Avalonia.Extensions;
using Stellarity.Avalonia.Models;
using Stellarity.Avalonia.ViewModel;
using Stellarity.Database.Entities;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.ViewModels.Pages;

public partial class AddGameViewModel : PageViewModel
{
    /// <summary>
    /// Viewmodel to resolve view for <see cref="IDialogService"/>
    /// </summary>
    private readonly MainViewModel _windowOwner = null!;

    /// <summary>
    /// Service to show dialog windows
    /// </summary>
    private readonly IDialogService _dialogService = null!;

    private readonly NavigationPublisher _navigator = null!;

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

    private byte[] _coverData = null!;

    private AddGameViewModel()
    {
        Free = true;
        Cover = ImagePlaceholder.GetBitmap();
        Validator = GetValidator();
    }

    public AddGameViewModel(MainViewModel windowOwner, IDialogService dialogService,
        NavigationPublisher navigationPublisher) : this()
    {
        _windowOwner = windowOwner;
        _dialogService = dialogService;
        _navigator = navigationPublisher;
    }

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<AddGameViewModel>();

        builder.RuleFor(vm => vm.Title)
            .NotEmpty()
            .MaxLength(25);

        builder.RuleFor(vm => vm.Description)
            .NotEmpty();

        builder.RuleFor(vm => vm.Developer)
            .NotEmpty()
            .MaxLength(30);

        builder.RuleFor(vm => vm.Cost)
            .Between(0, 5000);

        return builder.Build(this);
    }

    public Bitmap Cover
    {
        get => _coverData.ToBitmap()!;
        set
        {
            var data = value.FromBitmap();
            if (EqualityComparer<byte[]?>.Default.Equals(_coverData, data)) return;

            SetProperty(ref _coverData!, data);
            AddCommand.NotifyCanExecuteChanged();
        }
    }

    public bool CanAdd => Validator?.IsValid ?? false;

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

    [RelayCommand(CanExecute = nameof(CanAdd))]
    private async Task AddAsync()
    {
        if (Free) Cost = 0;
        await GameEntity.AddAsync(Title, Description, Developer, Cost, _coverData);
        _navigator.RaiseNavigated(this, NavigatedEventArgs.Pop());
    }
}