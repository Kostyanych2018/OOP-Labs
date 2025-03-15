using System.Collections.ObjectModel;
using BankingApp.DTOs;
using BankingApp.Entities.Customers;
using BankingApp.Entities.Users;

namespace BankingApp.Services.RegistrationService;

public interface IRegistrationService
{
    public ObservableCollection<User> GetUsers();
    public User? GetUserById(Guid userId);
    public Customer? GetCustomerById(Guid userId);
    ObservableCollection<RegistrationRequest> GetRegistrationRequests();
    void SubmitRegistration(RegistrationRequest registrationRequest); 
    void ApproveRegistration(RegistrationRequest registrationRequest);
    void RejectRegistration(RegistrationRequest registrationRequest);
}