using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using XboxAuthNet.OAuth;

namespace MinecraftLWPF.Minecraft
{
    public class AccountsManager : INotifyPropertyChanged
    {
        private static AccountsManager? _instance;
        public MSession currentSession { get; private set; } = null;
        private JELoginHandler loginhandler;
        private readonly HttpClient httpClient = new();
        public ObservableCollection<JEGameAccount> Accounts { get; private set; }

        // Private constructor to prevent instantiation from outside the class
        public AccountsManager()
        {
            _instance = this;
            Accounts = new ObservableCollection<JEGameAccount>();
            Login(true);

        }

        // Public method to get the singleton instance
        public static AccountsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AccountsManager();
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        public async Task Login(bool silently = false)
        {
            try
            {
                loginhandler = new JELoginHandlerBuilder()
                    .WithHttpClient(httpClient)
                    .WithAccountManager(MinecraftSettings.Instance.SettingsPath + "accounts.json")
                    .Build();

                //This means logging in with the Previous account if it was logged in before so we only need to fire the authentication event when 
                //the user is not logged in or hasn't logged in before
                if (silently)
                {
                    currentSession = await loginhandler.AuthenticateSilently();
                }
                else
                {
                    currentSession = await loginhandler.Authenticate();
                }
                await UpdateAccountList();
            }
            catch (MicrosoftOAuthException e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task Logout()
        {
            Console.WriteLine(currentSession.Xuid);
            await loginhandler.Signout(loginhandler.AccountManager.GetAccounts().GetAccount(currentSession.Xuid));
            currentSession = null;
            await UpdateAccountList();
        }
        private Task UpdateAccountList()
        {
            var accounts = loginhandler.AccountManager.GetAccounts();
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
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
