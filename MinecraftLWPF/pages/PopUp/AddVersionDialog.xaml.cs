using System.Windows;
using MinecraftLWPF.Minecraft;
using Wpf.Ui.Controls;
using Button = System.Windows.Controls.Button;

namespace MinecraftLWPF.pages.PopUp;

public partial class AddVersionDialog : Flyout
{
    public AddVersionDialog()
    {
        InitializeComponent();
        DataContext = LMinecraft.Instance; // Assuming L_Minecraft provides the MinecraftModel
    }

    private void Checkboxes_OnClick(object sender, RoutedEventArgs e)
    {
        LMinecraft.Instance.FilterVersions();
    }

    private void Dashboard_OnLoaded(object sender, RoutedEventArgs e)
    {
        LMinecraft.Instance.FilterVersions();
    }

    private void SexButton(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var version = Test.SelectedItem as MVersion;
        LMinecraft.Instance.DownloadVersion(version);
    }
}