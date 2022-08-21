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
            var profile = this.GetControl<NavigationViewItem>(nameof(Profile));
            profile.Content = new MyProfileView(App.Instance.DiContainer.Get<MyProfileViewModel>());
        });

        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        var border = this.GetControl<Border>(nameof(DragBorder));
        border.PointerPressed += (_, e) => BeginMoveDrag(e);
    }


    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}