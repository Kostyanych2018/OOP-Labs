using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BankingApp.Entities.Customers;
using BankingApp.Entities.Users;
using BankingApp.Services.AccountService;
using BankingApp.Services.AuthorizationService;
using BankingApp.Services.NavigationService;
using BankingApp.Services.RegistrationService;

// using JetBrains.Annotations;

namespace BankingApp.UI.ViewModels.RoleBasedModels;

public class ClientProfileViewModel : INotifyPropertyChanged
{
    private readonly INavigationService _navigationService;
    private readonly IRegistrationService _registrationService;
    
    private User? _currentUser;
    private Customer? _customerInfo;
    
    
    private string _customerType = "";
    private bool _isPhysicalPerson = false;
    private bool _isEnterprise = false;
    private bool _isBank = false;
    private bool _isLoading;

//для физ лица
    private string _fullName = "";
    private string _passportSeries = "";
    private string _passportNumber = "";
    private string _phoneNumber = "";
    private string _email = "";

//для предприятия и банков
    private string _legalName = "";
    private string _legalAddress = "";
    private string _taxIdNumber = "";
    private string _legalEntityType = "";
    private string _industryType = "";
    private string _bankIdCode = "";

    public ICommand LogoutCommand { get; }

    public ClientProfileViewModel(
        IRegistrationService registrationService,
        INavigationService navigationService,
        IAuthService authService)
    {
        _registrationService = registrationService;
        _navigationService = navigationService;
        CurrentUser = authService.CurrentUser;

        LogoutCommand = new Command(async () => await Logout());

        LoadClientData();
    }

    public async Task Logout()
    {
        bool confirm = await Shell.Current.DisplayAlert("Выход",
            "Вы уверены, что хотите выйти из системы?",
            "Да", "Нет");
        if (confirm)
        {
            CurrentUser = null;
            await _navigationService.GoBackToRootAsync();
        }
    }
    
    private void LoadClientData()
    {
        try
        {
            IsLoading = true;
            if (CurrentUser != null)
            {
                _customerInfo = _registrationService.GetUserById(CurrentUser.UserId);
                if (_customerInfo != null)
                {
                    switch (CurrentUser.CustomerType)
                    {
                        case DTOs.CustomerType.PhysicalPerson:
                            IsPhysicalPerson = true;
                            _customerType = "Физическое лицо";
                            var person = _customerInfo as PhysicalPerson;
                            if (person != null)
                            {
                                FullName = person.FullName ?? "";
                                PassportSeries = person.PassportSeries ?? "";
                                PassportNumber = person.PassportNumber ?? "";
                                PhoneNumber = person.PhoneNumber ?? "";
                                Email = person.Email ?? "";
                            }

                            break;
                        case DTOs.CustomerType.Enterprise:
                            _isEnterprise = true;
                            _customerType = "Предприятие";
                            var enterprise = _customerInfo as Enterprise;
                            if (enterprise != null)
                            {
                                LegalName = enterprise.LegalName ?? "";
                                LegalAddress = enterprise.LegalAddress ?? "";
                                TaxIdNumber = enterprise.TaxIdNumber ?? "";
                                LegalEntityType = enterprise.LegalEntityType ?? "";
                                IndustryType = enterprise.IndustryType ?? "";
                            }

                            break;
                        case DTOs.CustomerType.Bank:
                            _isBank = true;
                            _customerType = "Банк";
                            var bank = _customerInfo as Bank;
                            if (bank != null)
                            {
                                LegalName = bank.LegalName ?? "";
                                LegalAddress = bank.LegalAddress ?? "";
                                TaxIdNumber = bank.TaxIdNumber ?? "";
                                LegalEntityType = bank.LegalEntityType ?? "";
                                BankIdCode = bank.BankIdCode ?? "";
                            }

                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert($"Error loading customer data", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public User? CurrentUser
    {
        get => _currentUser;
        private set => SetProperty(ref _currentUser, value);
    }

    public string CustomerType => _customerType;

    protected bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string FullName
    {
        get => _fullName;
        set => SetProperty(ref _fullName, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public bool IsPhysicalPerson
    {
        get => _isPhysicalPerson;
        set => SetProperty(ref _isPhysicalPerson, value);
    }

    public bool IsEnterprise
    {
        get => _isEnterprise;
        set => SetProperty(ref _isEnterprise, value);
    }

    public bool IsBank
    {
        get => _isBank;
        set => SetProperty(ref _isBank, value);
    }

    public string PassportSeries
    {
        get => _passportSeries;
        set => SetProperty(ref _passportSeries, value);
    }

    public string PassportNumber
    {
        get => _passportNumber;
        set => SetProperty(ref _passportNumber, value);
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    public string LegalName
    {
        get => _legalName;
        set => SetProperty(ref _legalName, value);
    }

    public string LegalAddress
    {
        get => _legalAddress;
        set => SetProperty(ref _legalAddress, value);
    }

    public string TaxIdNumber
    {
        get => _taxIdNumber;
        set => SetProperty(ref _taxIdNumber, value);
    }

    public string LegalEntityType
    {
        get => _legalEntityType;
        set => SetProperty(ref _legalEntityType, value);
    }

    public string IndustryType
    {
        get => _industryType;
        set => SetProperty(ref _industryType, value);
    }

    public string BankIdCode
    {
        get => _bankIdCode;
        set => SetProperty(ref _bankIdCode, value);
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