using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftLWPF.pages;

public partial class Dashboard : Page
{
    public Dashboard()
    {
        InitializeComponent();
    }
    

    private void Checkboxes_OnClick(object sender, RoutedEventArgs e)
    {
        var checkbox = (CheckBox) sender;
        Debug.WriteLine(checkbox.Name);
    }
}