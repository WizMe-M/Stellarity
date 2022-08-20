using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ninject;
using Stellarity.Ninject;
using Stellarity.Views;

namespace Stellarity;

// ReSharper disable once ClassNeverInstantiated.Global
public class App : Application
{
    public static App Instance => (App)Current!;

    public App()
    {
        DiContainer = new StandardKernel(new DialogServiceModule(),
            new ViewModelModule());
        DiContainer.Settings.AllowNullInjection = true;
    }

    public StandardKernel DiContainer { get; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Task.Run(StellarisContext.CreateDatabaseAsync);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
#if DEBUG
            desktop.MainWindow = new AuthorizationView();
#else
            desktop.MainWindow = new AuthorizationView();
#endif
        }

        base.OnFrameworkInitializationCompleted();
    }
}