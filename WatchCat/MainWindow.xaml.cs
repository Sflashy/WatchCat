using System;
using System.Windows;
using System.Windows.Input;

namespace WatchCat
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Source = new Uri("Items.xaml", UriKind.RelativeOrAbsolute);
            UpdateManager.CheckForUpdates();
        }

        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnWindowStateChanged(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
                //
            }
        }

        private void TopMostToggle(object sender, MouseButtonEventArgs e)
        {
            Topmost = !Topmost;
        }
    }
}
