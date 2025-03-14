using System.Collections.ObjectModel;
using BankingApp.Entities.Users;
using BankingApp.Services.RegistrationService;

namespace BankingApp.Services.AdminPanelService;

    public class AdminPanelService : IAdminPanelService
    {
        private readonly IRegistrationService _registrationService;

        public AdminPanelService(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public ObservableCollection<User> GetAllUsers()
        {
            return _registrationService.GetUsers();
        }

        public void AssignRole(Guid userId, UserRole role)
        {
            var users = _registrationService.GetUsers();
            var user = users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.Role = role;
            }
            else
            {
                throw new InvalidOperationException("Пользователь не найден");
            }
        }
    }