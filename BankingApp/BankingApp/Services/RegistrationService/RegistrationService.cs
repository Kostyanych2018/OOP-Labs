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

        var banks = DataGenerationService.GenerateBanks(3);
        var enterprises = DataGenerationService.GenerateEnterprises(3);
        var persons = DataGenerationService.GeneratePhysicalPersons(5);
        var generatedUsers = DataGenerationService.GenerateUsers(banks, enterprises, persons);
        _users = new ObservableCollection<User>(generatedUsers);
        // InitializeTestData();
    }

    public User? GetUserById(Guid userId)
    {
        return _users.FirstOrDefault(u=>u.UserId==userId);
    }

    public Customer? GetCustomerById(Guid userId)
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

    // public void InitializeTestData()
    // {
    //     var testRequests = new List<RegistrationRequest>()
    //     {
    //       
    //         }
    //     };
    //     foreach (var request in testRequests)
    //     {
    //         _registrationRequests.Add(request);
    //     }
    // }
}