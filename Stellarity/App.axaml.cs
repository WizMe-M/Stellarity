using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ninject;
using Stellarity.Database.Entities;
using Stellarity.Ninject;
using Stellarity.Services.Accounting;
using Stellarity.ViewModels;
using Stellarity.Views;

namespace Stellarity;

// ReSharper disable once ClassNeverInstantiated.Global
public class App : Application
{
    public App()
    {
        DiContainer = new StandardKernel(new DialogServiceModule(),
            new ServicesModule(), new ViewModelModule());
        DiContainer.Settings.AllowNullInjection = true;
    }

    public new static App Current => (App)Application.Current!;

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
            var service = DiContainer.Get<AccountingService>();
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