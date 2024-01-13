using System.Net;
using System.Windows;
using MinecraftLWPF.pages;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace MinecraftLWPF;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
    private readonly IThemeService _themeService;

    public MainWindow()
    {
        InitializeComponent();
        _themeService = new ThemeService();

        ServicePointManager.DefaultConnectionLimit = 256;

        DataContext = this;
        SystemThemeWatcher.Watch(this);

        InitializeComponent();

        Loaded += (_, _) => RootNavigation.Navigate(typeof(Dashboard));
    }

    private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
    {
        _themeService.SetTheme(_themeService.GetTheme() == ApplicationTheme.Dark
            ? ApplicationTheme.Light
            : ApplicationTheme.Dark);
    }
}