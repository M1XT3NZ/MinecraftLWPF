using System;
using System.Windows;
using System.Windows.Controls;
using MinecraftLWPF.Minecraft;

// Other using statements

namespace MinecraftLWPF.pages;

public partial class AccountsPage : Page
{
    private readonly AccountsManager accountsManager;

    public AccountsPage()
    {
        InitializeComponent();
        accountsManager = AccountsManager.Instance;
        // Additional initialization if needed
    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await accountsManager.Login();
            // After successful login, fetch and display character head
            await accountsManager.RefreshHead();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }
    private async void Logout_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await accountsManager.Logout();
            // After successful login, fetch and display character head
            await accountsManager.RefreshHead();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }
}