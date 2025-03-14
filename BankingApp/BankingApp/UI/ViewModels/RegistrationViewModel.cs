using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BankingApp.DTOs;
using BankingApp.Entities.Users;
using BankingApp.Services.RegistrationService;

namespace BankingApp.UI.ViewModels;

public class RegistrationViewModel : INotifyPropertyChanged
{
    private readonly IRegistrationService _registrationService;

    private string? _username;
    private string? _password;
    private string? _email;

    private string? _fullName;
    private string? _passportSeries;
    private string? _passportNumber;
    private string? _phoneNumber;

    private string? _legalName;
    private string? _legalAddress;
    private string? _taxIdNumber;
    private string? _legalEntityType;

    private string? _bankIdCode;
    private string? _industryType;


    private CustomerType _selectedCustomerType;
    // private UserRole _selectedRole;
    private string? _statusMessage;

    public RegistrationViewModel(IRegistrationService registrationService)
    {
        _bankIdCode = null;
        _registrationService = registrationService;
        RegisterCommand = new Command(Register, CanRegister);
        //списки для выбора
        CustomerTypes = Enum.GetValues<CustomerType>().ToList();
        SelectedCustomerType = CustomerTypes.First();
    }

    public ICommand RegisterCommand { get; }
    public List<CustomerType> CustomerTypes { get; set; }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool CanRegister()
    {
        bool hasBasicInfo = !string.IsNullOrWhiteSpace(Username) &&
                            !string.IsNullOrWhiteSpace(Password);
                            // !string.IsNullOrWhiteSpace(Email);
        switch (_selectedCustomerType)
        {
            case CustomerType.PhysicalPerson:
                return hasBasicInfo &&
                       !string.IsNullOrWhiteSpace(FullName) &&
                       !string.IsNullOrWhiteSpace(PassportNumber);
            case CustomerType.Bank:
                return hasBasicInfo &&
                       !string.IsNullOrWhiteSpace(LegalName) &&
                       !string.IsNullOrWhiteSpace(TaxIdNumber) &&
                       !string.IsNullOrWhiteSpace(BankIdCode);
            case CustomerType.Enterprise:
                return hasBasicInfo &&
                       !string.IsNullOrWhiteSpace(LegalName) &&
                       !string.IsNullOrWhiteSpace(TaxIdNumber);
            default:
                return false;
        }
    }

    private void Register()
    {
        var request = new RegistrationRequest()
        {
            Username = Username,
            Password = Password,
            Email = Email,
            CustomerType = SelectedCustomerType,
        };
        switch (SelectedCustomerType)
        {
            case CustomerType.PhysicalPerson:
                request.FullName = FullName;
                request.PassportSeries = PassportSeries;
                request.PassportNumber = PassportNumber;
                request.PhoneNumber = PhoneNumber;
                request.Email = Email;
                break;
            case CustomerType.Bank:
                request.LegalName = LegalName;
                request.LegalAddress = LegalAddress;
                request.TaxIdNumber = TaxIdNumber;
                request.LegalEntityType = LegalEntityType;
                request.BankIdCode = BankIdCode;
                request.Email = Email;
                break;
            case CustomerType.Enterprise:
                request.LegalName = LegalName;
                request.LegalAddress = LegalAddress;
                request.TaxIdNumber = TaxIdNumber;
                request.LegalEntityType = LegalEntityType;
                request.IndustryType = IndustryType;
                request.Email = Email;
                break;
        }

        _registrationService.SubmitRegistration(request);
        StatusMessage = "Запрос на регистрацию отправлен. Ожидайте согласия менеджера.";
        Shell.Current.DisplayAlert("Регистрация", "Запрос на регистрацию отправлен. " +
                                                  "Менеджер рассмотрит ваш запрос.", "OK");
    }

    public bool CanExecuteRegister => CanRegister();

    public string? Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string? Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string? Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string? FullName
    {
        get => _fullName;
        set => SetProperty(ref _fullName, value);
    }

    public string? PassportSeries
    {
        get => _passportSeries;
        set => SetProperty(ref _passportSeries, value);
    }

    public string? PassportNumber
    {
        get => _passportNumber;
        set => SetProperty(ref _passportNumber, value);
    }

    public string? PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    public string? LegalName
    {
        get => _legalName;
        set => SetProperty(ref _legalName, value);
    }

    public string? LegalAddress
    {
        get => _legalAddress;
        set => SetProperty(ref _legalAddress, value);
    }

    public string? TaxIdNumber
    {
        get => _taxIdNumber;
        set => SetProperty(ref _taxIdNumber, value);
    }

    public string? LegalEntityType
    {
        get => _legalEntityType;
        set => SetProperty(ref _legalEntityType, value);
    }

    public string? BankIdCode
    {
        get => _bankIdCode;
        set => SetProperty(ref _bankIdCode, value);
    }

    public string? IndustryType
    {
        get => _industryType;
        set => SetProperty(ref _industryType, value);
    }

    public CustomerType SelectedCustomerType
    {
        get => _selectedCustomerType;
        set => SetProperty(ref _selectedCustomerType, value);
    }
    
    public string? StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        OnPropertyChanged(nameof(CanExecuteRegister));
        ((Command)RegisterCommand).ChangeCanExecute();
        return true;
    }
}