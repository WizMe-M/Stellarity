using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HanumanInstitute.MvvmDialogs;
using Ninject;
using ReactiveUI;
using Stellarity.Services.Accounting;
using Stellarity.UserControls;
using Stellarity.ViewModels;
using Stellarity.ViewModels.Pages;
using Stellarity.Views.Pages;

namespace Stellarity.Views;

public partial class MainView : ReactiveWindow<MainViewModel>
{
    public MainView()
    {
        this.WhenActivated(d =>
        {
            var accountingService = App.Current.DiContainer.Get<AccountingService>();
            Profile = this.GetControl<NavigationViewItem>(nameof(Profile));
            Profile.Content = new MyProfileView(new MyProfileViewModel(accountingService));

            var dialogService = App.Current.DiContainer.Get<IDialogService>();
            EditProfile = this.GetControl<NavigationViewItem>(nameof(EditProfile));
            EditProfile.Content =
                new EditProfileView(new EditProfileViewModel(dialogService, ViewModel!, accountingService.AuthorizedAccount!));
        });

        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        DragBorder = this.GetControl<Border>(nameof(DragBorder));
        DragBorder.PointerPressed += (_, e) => BeginMoveDrag(e);
    }


    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}