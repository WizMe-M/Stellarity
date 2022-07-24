using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.ViewModels;

namespace Stellarity.Views;

public partial class MainView : ReactiveWindow<MainViewModel>
{
    public MainView()
    {
        this.WhenActivated(d => { });
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    
    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}