using System.Collections.ObjectModel;
using BankingApp.DTOs;
using BankingApp.Entities.Customers;
using BankingApp.Entities.Users;

namespace BankingApp.Services.RegistrationService;

//управление заявками на регистрацию клиентов
public class RegistrationService : IRegistrationService
{
    private readonly ObservableCollection<RegistrationRequest> _registrationRequests = [];
    private readonly ObservableCollection<User> _users;

    public RegistrationService()
    {
        Customer bank1 = new Bank()
        {
            LegalName = "Сбербанк",
            LegalAddress = "ул. Ленина, д. 1",
            TaxIdNumber = "111222333",
            LegalEntityType = "ООО",
            BankIdCode = "SB001",
            Email = "contact@sberbank.ru",
        };
        Customer bank2 = new Bank()
        {
            LegalName = "ВТБ",
            LegalAddress = "пр. Мира, д. 5",
            TaxIdNumber = "444555666",
            LegalEntityType = "ОАО",
            BankIdCode = "VTB002",
            Email = "contact@vtb.ru",
        };
        _users = new ObservableCollection<User>()
        {
            new User()
            {
                UserId = Guid.NewGuid(),
                Username = "sberbank",
                Password = "123",
                Customer = bank1,
                CustomerType = CustomerType.Bank
            },
            new User()
            {
            UserId = Guid.NewGuid(),
            Username = "vtb",
            Password = "345",
            Customer = bank2,
            CustomerType = CustomerType.Bank
            
            }
        };
        InitializeTestData();
    }

    public Customer? GetUserById(Guid userId)
    {
        return _users.FirstOrDefault(u=>u.UserId==userId)?.Customer;
    }
    public ObservableCollection<User> GetUsers()
    {
        return _users;
    }
    public ObservableCollection<RegistrationRequest> GetRegistrationRequests()
    {
        return _registrationRequests;
    }

    public void SubmitRegistration(RegistrationRequest request)
    {
        _registrationRequests.Add(request);
    }

    public void ApproveRegistration(RegistrationRequest request)
    {
        Customer? customer = null;

        switch (request.CustomerType)
        {
            case CustomerType.Bank:
                customer = new Bank()
                {
                    Email = request.Email,
                    LegalName = request.LegalName,
                    LegalAddress = request.LegalAddress,
                    TaxIdNumber = request.TaxIdNumber,
                    LegalEntityType = request.LegalEntityType,
                    BankIdCode = request.BankIdCode,
                };
                break;
            case CustomerType.Enterprise:
                customer = new Enterprise()
                {
                    IndustryType = request.IndustryType,
                    LegalName = request.LegalName,
                    LegalAddress = request.LegalAddress,
                    TaxIdNumber = request.TaxIdNumber,
                    LegalEntityType = request.LegalEntityType,
                };
                break;
            case CustomerType.PhysicalPerson:
                customer = new PhysicalPerson()
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    PassportSeries = request.PassportSeries,
                    PassportNumber = request.PassportNumber,
                    PhoneNumber = request.PhoneNumber,
                };
                break;
        }

        if (customer == null) return;
        var user = new User()
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            Password = request.Password,
            CustomerType = request.CustomerType,
            Customer = customer
        };
        _users.Add(user);
        request.Status = RegistrationStatus.Approved;
    }

    public void RejectRegistration(RegistrationRequest request)
    {
        request.Status = RegistrationStatus.Rejected;
    }

    public void InitializeTestData()
    {
        var testRequests = new List<RegistrationRequest>()
        {
            new RegistrationRequest()
            {
                Username = "bank_user",
                Password = "123",
                Email = "bank@example.com",
                CustomerType = CustomerType.Bank,
                LegalName = "Test Bank",
                LegalAddress = "123 Main Street",
                TaxIdNumber = "123456789",
                LegalEntityType = "Bank",
                BankIdCode = "BANK123",
                Status = RegistrationStatus.Pending
            },
            new RegistrationRequest()
            {
                Username = "enterprise_user",
                Password = "enterprise_password",
                CustomerType = CustomerType.Enterprise,
                LegalName = "Test enterprise",
                LegalAddress = "123 Enterprise Street",
                TaxIdNumber = "143948389",
                LegalEntityType = "enterprise",
                Status = RegistrationStatus.Pending
            },
            new RegistrationRequest()
            {
                Username = "John_Doe",
                Password = "123",
                PassportSeries = "AB",
                PassportNumber = "1232339",
                PhoneNumber = "+123-456-789",
                CustomerType = CustomerType.PhysicalPerson,
                Status = RegistrationStatus.Pending
            }
        };
        foreach (var request in testRequests)
        {
            _registrationRequests.Add(request);
        }
    }
}