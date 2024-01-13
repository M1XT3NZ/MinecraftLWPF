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
        accountsManager = new AccountsManager();
        // Additional initialization if needed
    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await accountsManager.Login();
            // After successful login, fetch and display character head
            UpdateCharacterHead();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private void UpdateCharacterHead()
    {
        // Implement logic to fetch character's head based on the account
        // Update CharacterHeadImage.Source with the fetched image
    }
}