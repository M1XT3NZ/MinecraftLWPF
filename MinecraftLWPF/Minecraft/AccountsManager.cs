using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;

namespace MinecraftLWPF.Minecraft
{
    public class AccountsManager : INotifyPropertyChanged
    {
        private static AccountsManager? _instance;
        private MSession currentSession = null;
        private JELoginHandler loginhandler;
        private readonly HttpClient httpClient = new();

        // Private constructor to prevent instantiation from outside the class
        public AccountsManager()
        {
            Instance = this;
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

        public string PlayerName
        {
            get
            {
                if (currentSession != null) return currentSession.Username;
                return null;
            }
        }

        public async Task Login()
        {
            loginhandler = new JELoginHandlerBuilder()
                .WithHttpClient(httpClient)
                .WithAccountManager(MinecraftSettings.Instance.SettingsPath + "accounts.json")
                .Build();
            currentSession = await loginhandler.Authenticate();
        }

        public async Task Logout()
        {
            Console.WriteLine(currentSession.Xuid);
            await loginhandler.Signout(loginhandler.AccountManager.GetAccounts().GetAccount(currentSession.Xuid));
            currentSession = null;
        }

        public Task RefreshHead()
        {
            OnPropertyChanged(nameof(PlayerName));
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
