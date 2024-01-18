using System;
using System.Windows;
using System.Windows.Controls;
using MinecraftLWPF.Minecraft;

// Other using statements

namespace MinecraftLWPF.pages;

public partial class AccountsPage : Page
{
    private readonly AccountsManager _accountsManager;

    public AccountsPage()
    {
        InitializeComponent();
        _accountsManager = AccountsManager.Instance;
        // Additional initialization if needed
    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await _accountsManager.Login();
            // After successful login, fetch and display character head
           // await accountsManager.RefreshHead();
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
            await _accountsManager.Logout();
            // After successful login, fetch and display character head
            //await accountsManager.RefreshHead();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }
}