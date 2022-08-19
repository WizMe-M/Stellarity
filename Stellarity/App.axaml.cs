using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Stellarity.Database;
using Stellarity.Views;

namespace Stellarity;

public class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        Task.Run(StellarisContext.CreateDatabaseAsync);

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