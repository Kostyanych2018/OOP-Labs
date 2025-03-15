using System.Collections.ObjectModel;
using BankingApp.Entities.Users;

namespace BankingApp.Services.SystemAdminService;

public interface ISystemAdminService
{
    ObservableCollection<User> GetAllUsers();
    void AssignRole(Guid userId, UserRole newRole);
}