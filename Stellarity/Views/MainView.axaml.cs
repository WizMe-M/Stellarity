using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Ninject;
using ReactiveUI;
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
            Profile = this.GetControl<NavigationViewItem>(nameof(Profile));
            Profile.Content = new MyProfileView(App.Instance.DiContainer.Get<MyProfileViewModel>());
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