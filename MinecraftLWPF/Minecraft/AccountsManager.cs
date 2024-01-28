using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth;

namespace MinecraftLWPF.Minecraft
{
    public static class AccountsManager
    {
        public static MSession? CurrentSession { get; set; } = new MSession();

        private static JEGameAccount? _selectedAccount;

        public static JEGameAccount? SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnSelectedAccountChanged?.Invoke();
                RefreshHead();
            }
        }
        public static Action? OnSelectedAccountChanged;
        private static JELoginHandler? _loginhandler;
        private static readonly HttpClient _httpClient = new();
        public static ObservableCollection<JEGameAccount>? Accounts { get; private set; } = new ObservableCollection<JEGameAccount>();

        public static async Task Login(bool silently = false)
        {
            OnSelectedAccountChanged += async () => await RefreshHead();
            try
            {
                _loginhandler = new JELoginHandlerBuilder()
                    .WithHttpClient(_httpClient)
                    .WithAccountManager(MinecraftSettings.Instance.SettingsPath + "accounts.json")
                    .Build();

                //This means logging in with the Previous account if it was logged in before so we only need to fire the authentication event when 
                //the user is not logged in or hasn't logged in before
                if (silently)
                    CurrentSession = await _loginhandler.AuthenticateSilently();
                else
                    CurrentSession = await _loginhandler.Authenticate();
                await UpdateAccountList();
            }
            catch (MicrosoftOAuthException e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task Logout()
        {
            Console.WriteLine(CurrentSession.Xuid);
            await _loginhandler.Signout(_loginhandler.AccountManager.GetAccounts().GetAccount(CurrentSession.Xuid));
            CurrentSession = null;
            await UpdateAccountList();
        }

        private static Task UpdateAccountList()
        {
            var accounts = _loginhandler.AccountManager.GetAccounts();
            foreach (var account in accounts)
            {
                if (account is not JEGameAccount jeAccount)
                    continue;
                Console.WriteLine("Identifier: " + jeAccount.Identifier);
                Console.WriteLine("LastAccess: " + jeAccount.LastAccess);
                Console.WriteLine("Gamertag: " + jeAccount.XboxTokens?.XstsToken?.XuiClaims?.Gamertag);
                Console.WriteLine("Username: " + jeAccount.Profile?.Username);
                Console.WriteLine("UUID: " + jeAccount.Profile?.UUID);
                Accounts.Add(jeAccount);
            }

            return Task.CompletedTask;
        }
        
        public static async Task RefreshHead()
        {
            if (CurrentSession == null)
                return;
            AccountsManagerCommands.Instance.SelectedAccountUsername = CurrentSession.Username;
            
        }
    }
    

}