using CommunityToolkit.Mvvm.DependencyInjection;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Windows;
using System.Windows.Threading;
using WatchCat.Core.Services;
using WatchCat.ViewModels;

namespace WatchCat;

public partial class App : Application
{
    [STAThread]
    public static void Main(string[] args)
    {
        InitializeServices();
        InitializeConfiguration();
        InitializeApp();
    }

    private static void InitializeApp()
    {
        App app = new App();
        app.InitializeComponent();

        app.MainWindow = Ioc.Default.GetRequiredService<MainView>();
        app.MainWindow.Visibility = Visibility.Visible;
        Log.Information("WatchCat Initialized");

        app.Run();
    }

    private static void InitializeConfiguration()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();
    }
    private static void InitializeServices()
    {
        var services = new ServiceCollection();

        //views
        services.AddSingleton<MainView>();

        //viewModels
        services.AddTransient<MainViewModel>();

        //services
        services.AddTransient<HttpClientService>();
        services.AddSingleton(_ => Current.Dispatcher);
        services.AddTransient<ISnackbarMessageQueue>(provider =>
        {
            Dispatcher dispatcher = provider.GetRequiredService<Dispatcher>();
            return new SnackbarMessageQueue(TimeSpan.FromSeconds(2.0), dispatcher);
        });
        Ioc.Default.ConfigureServices(services.BuildServiceProvider());
    }


}
