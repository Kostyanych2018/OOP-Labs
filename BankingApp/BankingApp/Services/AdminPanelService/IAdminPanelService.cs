using System.Collections.ObjectModel;
using BankingApp.Entities.Users;

namespace BankingApp.Services.AdminPanelService;

public interface IAdminPanelService
{
    ObservableCollection<User> GetAllUsers();
    void AssignRole(Guid userId, UserRole role);
}