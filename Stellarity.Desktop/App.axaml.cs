using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ninject;
using Stellarity.Database.Entities;
using Stellarity.Desktop.Ninject;
using Stellarity.Desktop.ViewModels;
using Stellarity.Desktop.Views;
using Stellarity.Services;
using Stellarity.Services.Accounting;

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
        // Task.Run(StellarisContext.CreateDatabaseAsync);
        DiContainingService.Initialize(new DialogServiceModule());
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
#if DEBUG
            var service = DiContainingService.Kernel.Get<AccountingService>();
            service.AuthorizedAccount = Account.GetFirst();
            desktop.MainWindow = new MainView
            {
                ViewModel = new MainViewModel()
            };
#else
            desktop.MainWindow = new AuthorizationView();
#endif
        }

        base.OnFrameworkInitializationCompleted();
    }
}