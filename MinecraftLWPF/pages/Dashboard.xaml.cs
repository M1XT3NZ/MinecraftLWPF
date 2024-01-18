using System.Windows;
using System.Windows.Controls;
using MinecraftLWPF.Minecraft;
using MinecraftLWPF.pages.PopUp;
using Wpf.Ui.Controls;

namespace MinecraftLWPF.pages;

public partial class Dashboard : Page
{
    public Dashboard()
    {
        InitializeComponent();
        DataContext = LMinecraft.Instance;
        TheFly = new AddVersionDialog();
    }

    private void Dashboard_OnLoaded(object sender, RoutedEventArgs e)
    {
        LMinecraft.Instance.GetVersionsLocal();
    }

    private void AddVersion_Click(object sender, RoutedEventArgs e)
    {
        LMinecraft.Instance.IsFlyoutOpen = true;
    }

    private void TheFly_OnClosed(Flyout sender, RoutedEventArgs args)
    {
        LMinecraft.Instance.IsFlyoutOpen = false;
    }
}