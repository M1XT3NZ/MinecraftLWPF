using System.Windows;
using System.Windows.Controls;
using MinecraftLWPF.Minecraft;

namespace MinecraftLWPF.pages;

public partial class Dashboard : Page
{
    public Dashboard()
    {
        InitializeComponent();
        DataContext = L_Minecraft.Instance;
    }
    private void Checkboxes_OnClick(object sender, RoutedEventArgs e)
    {
        L_Minecraft.Instance.FilterVersions();
    }
    private void Dashboard_OnLoaded(object sender, RoutedEventArgs e)
    {
        L_Minecraft.Instance.FilterVersions();
    }
}