using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BankingApp.DTOs;
using BankingApp.Entities.Banking;
using BankingApp.Entities.Customers;
using BankingApp.Entities.Users;
using BankingApp.Services.AccountService;
using BankingApp.Services.AuthorizationService;
using BankingApp.Services.RegistrationService;

namespace BankingApp.UI.ViewModels.RoleBasedModels;

public class ClientAccountsViewModel : INotifyPropertyChanged
{
    private readonly IAccountService _accountService;
    private readonly IRegistrationService _registrationService;

    private ObservableCollection<Account> _accounts = [];
    private ObservableCollection<Transaction> _transactions = [];
    private ObservableCollection<Bank> _availableBanks = [];


    private Account? _selectedAccount;
    private User? _currentUser;
    private Bank? _selectedBank;

    private string _errorMessage = "";
    private string _recipientAccountNumber = "";
    private decimal _transferAmount;
    private decimal _depositAmount;
    private decimal _withdrawAmount;

    private AccountType _selectedAccountType;

    public ICommand OpenAccountCommand { get; }
    public ICommand CloseAccountCommand { get; }
    public ICommand DepositCommand { get; }
    public ICommand WithdrawCommand { get; }
    public ICommand TransferCommand { get; }


    public ClientAccountsViewModel(
        IAccountService accountService,
        IAuthService authService,
        IRegistrationService registrationService)
    {
        _accountService = accountService;
        _registrationService = registrationService;

        CurrentUser = authService.CurrentUser;

        OpenAccountCommand = new Command(async () => await OpenAccount());
        CloseAccountCommand = new Command(async () => await CloseAccount());
        DepositCommand = new Command(async () => await Deposit());
        WithdrawCommand = new Command(async () => await Withdraw());
        TransferCommand = new Command(async () => await Transfer());
        LoadBanks();
    }

    private void LoadBanks()
    {
        try
        {
            ErrorMessage = "";
            var users = _registrationService.GetUsers();
            var banks = users.Where(u => u.CustomerType == CustomerType.Bank)
                .Select(u => u.Customer as Bank)
                .Where(b => b != null)
                .ToList();
            AvailableBanks = new ObservableCollection<Bank>(banks!);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка при загрузке банков: {ex.Message}";
            Shell.Current.DisplayAlert("Ошибка", ErrorMessage, "OK");
        }
    }

    public async Task LoadAccounts()
    {
        try
        {
            ErrorMessage = "";
            if (CurrentUser == null || SelectedBank == null) return;
            var accounts = await _accountService.GetAccounts(CurrentUser.UserId);
            Accounts = new ObservableCollection<Account>(accounts.Where(a => a.BankIdCode == SelectedBank.BankIdCode));
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка при загрузке счетов:{ex.Message}";
            await Shell.Current.DisplayAlert("Ошибка", _errorMessage, "OK");
        }
    }

    public async Task OpenAccount()
    {
        if (CurrentUser == null || SelectedBank == null) return;
        try
        {
            string result = await Shell.Current.DisplayActionSheet(
                "Выберите тип счета", "Отмена", null,
                "Накопительный счет", "Расчетный счет", "Зарплатный счет");

            if (result == "Отмена" || string.IsNullOrEmpty(result)) return;

            switch (result)
            {
                case "Накопительный счет":
                    _selectedAccountType = AccountType.Savings;
                    break;
                case "Расчетный счет":
                    _selectedAccountType = AccountType.Checking;
                    break;
                case "Зарплатный счет":
                    _selectedAccountType = AccountType.Salary;
                    break;
                default:
                    _selectedAccountType = AccountType.Checking;
                    break;
            }

            var userId = CurrentUser.UserId;
            var newAccount = await _accountService.OpenAccount(userId, SelectedBank.BankIdCode, SelectedAccountType);
            await Shell.Current.DisplayAlert("Счет открыт",
                $"Новый счет №{newAccount.AccountNumber} успешно открыт", "ОK");
            await LoadAccounts();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось открыть счет: {ex.Message}", "OK");
        }
    }

    public async Task CloseAccount()
    {
        if (SelectedAccount == null) return;
        try
        {
            bool confirmed = await Shell.Current.DisplayAlert(
                "Подтверждение",
                $"Вы уверены, что хотите закрыть счет №{SelectedAccount.AccountNumber}?",
                "Да", "Нет");
            if (!confirmed) return;
            await _accountService.CloseAccount(SelectedAccount.AccountId);
            await Shell.Current.DisplayAlert("Успех", "Счет успешно закрыт", "OK");
            await LoadAccounts();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось закрыть счет: {ex.Message}", "OK");
        }
    }

    public async Task Deposit()
    {
        if (SelectedAccount == null) return;
        try
        {
            if (DepositAmount <= 0)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Сумма должна быть больше нуля", "OK");
                return;
            }

            await _accountService.Deposit(SelectedAccount.AccountId, DepositAmount);
            await Shell.Current.DisplayAlert("Успех", $"Внесено {DepositAmount:C} на счет №{SelectedAccount.AccountNumber}", "OK");
            await LoadAccounts();
            DepositAmount = 0;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось пополнить счет: {ex.Message}", "OK");
        }
    }

    public async Task Withdraw()
    {
        try
        {
            if (WithdrawAmount <= 0)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Сумма должна быть больше нуля", "OK");
                return;
            }

            if (WithdrawAmount > SelectedAccount!.Balance)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Недостаточно средств на счете", "OK");
                return;
            }

            var transaction = await _accountService.Withdraw(SelectedAccount.AccountId, WithdrawAmount);
            await Shell.Current.DisplayAlert("Успех", $"Снято {WithdrawAmount:C} со счета {SelectedAccount.AccountNumber}", "OK");
            await LoadAccounts();
            WithdrawAmount = 0;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось снять средства: {ex.Message}", "OK");
        }
    }

    public async Task Transfer()
    {
        if(SelectedAccount==null) return;
        try
        {
            if (TransferAmount <= 0)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Сумма должна быть больше нуля", "OK");
                return;
            }
            if (TransferAmount > SelectedAccount.Balance)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Недостаточно средств на счете", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(RecipientAccountNumber))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Введите номер счета получателя", "OK");
                return;
            }
            var accounts = await _accountService.GetAccounts(Guid.Empty);
            var destinationAccount = accounts.FirstOrDefault(a => a.AccountNumber == RecipientAccountNumber);
            if (destinationAccount == null)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Счет получателя не найден", "OK");
                return;
            }
            var transaction = await _accountService.Transfer(SelectedAccount.AccountId, destinationAccount.AccountId, TransferAmount);
            await Shell.Current.DisplayAlert("Успех", 
                $"Переведено {TransferAmount:C} со счета {SelectedAccount.AccountNumber} на счет {RecipientAccountNumber}", 
                "OK");
            await LoadAccounts();
            TransferAmount = 0;
            RecipientAccountNumber = "";
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось выполнить перевод: {ex.Message}", "OK");
        }
    }


    public User? CurrentUser
    {
        get => _currentUser;
        private set => SetProperty(ref _currentUser, value);
    }

    public Bank? SelectedBank
    {
        get => _selectedBank;
        set
        {
            if (SetProperty(ref _selectedBank, value))
            {
                _ = LoadAccounts();
            }
        }
    }

    public ObservableCollection<Bank> AvailableBanks
    {
        get => _availableBanks;
        set => SetProperty(ref _availableBanks, value);
    }
    public string RecipientAccountNumber
    {
        get => _recipientAccountNumber;
        set => SetProperty(ref _recipientAccountNumber, value);
    }

    public Account? SelectedAccount
    {
        get => _selectedAccount;
        set
        {
            SetProperty(ref _selectedAccount, value); 
            ((Command)CloseAccountCommand).ChangeCanExecute();
            ((Command)DepositCommand).ChangeCanExecute();
            ((Command)WithdrawCommand).ChangeCanExecute();
            ((Command)TransferCommand).ChangeCanExecute();
        }
    }

    public ObservableCollection<Account> Accounts
    {
        get => _accounts;
        set => SetProperty(ref _accounts, value);
    }

    public decimal DepositAmount
    {
        get => _depositAmount;
        set => SetProperty(ref _depositAmount, value);
    }

    public decimal WithdrawAmount
    {
        get => _withdrawAmount;
        set => SetProperty(ref _withdrawAmount, value);
    }

    public decimal TransferAmount
    {
        get => _transferAmount;
        set => SetProperty(ref _transferAmount, value);
    }

    public AccountType SelectedAccountType
    {
        get => _selectedAccountType;
        set => SetProperty(ref _selectedAccountType, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}