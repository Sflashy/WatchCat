using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;
using WatchCat.ViewModels;

namespace WatchCat;

public partial class MainView : Window
{
    public MainViewModel ViewModel { get; set; }
    public MainView()
    {
        InitializeComponent();
        ViewModel = Ioc.Default.GetRequiredService<MainViewModel>();
        DataContext = ViewModel;
    }
}
