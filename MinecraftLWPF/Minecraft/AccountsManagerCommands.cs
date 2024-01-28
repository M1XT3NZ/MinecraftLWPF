using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace MinecraftLWPF.Minecraft;

public class AccountsManagerCommands : INotifyPropertyChanged
{
    public static AccountsManagerCommands Instance { get; private set; }
    public ICommand SelectAccountCommand { get; }


        private string _selectedAccountUsername;
        public string SelectedAccountUsername
        {
            get => _selectedAccountUsername;
            set
            {
                _selectedAccountUsername = value;
                AccountsManager.SelectedAccount = AccountsManager.Accounts.FirstOrDefault(a => a.Profile.Username == value);
                OnPropertyChanged();
            }
        }
    
    public AccountsManagerCommands()
    {
        Instance = this;
    }

    // Implement INotifyPropertyChanged interface here
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
public class RelayCommand : ICommand
{
    private readonly Action _execute;

    public RelayCommand(Action execute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true; // You can implement your own logic here if needed
    }

    public void Execute(object parameter)
    {
        _execute();
    }
}

