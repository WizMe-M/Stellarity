using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Stellarity.Database;
using Stellarity.Desktop.Ninject;
using Stellarity.Desktop.Views;
using Stellarity.Domain.Ninject;

namespace Stellarity.Desktop;

// ReSharper disable once ClassNeverInstantiated.Global
public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        DatabaseInitializer.CreateDb();
        DiContainingService.Initialize(new DialogServiceModule(), new ServicesModule());

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new AuthorizationView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}