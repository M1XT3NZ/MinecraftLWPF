using System;
using System.Windows;
using System.Windows.Controls;
using CmlLib.Core.Auth.Microsoft.Sessions;
using MinecraftLWPF.Minecraft;

// Other using statements

namespace MinecraftLWPF.pages;

public partial class AccountsPage : Page
{

    public AccountsPage()
    {
        InitializeComponent();
        // Additional initialization if needed
    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await AccountsManager.Login();
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
            await AccountsManager.Logout();
            // After successful login, fetch and display character head
            //await accountsManager.RefreshHead();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }
    
    private async void AccountSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(e.AddedItems.Count == 0) return;
        // Get the selected account
        var selectedAccount = (JEGameAccount)e.AddedItems[0];

        // Perform your actions with the selected account
        // For example, you might want to log in with the selected account
        try
        {
            AccountsManager.SelectedAccount = selectedAccount;
            // After successful login, fetch and display character head
            // await accountsManager.RefreshHead();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }
}