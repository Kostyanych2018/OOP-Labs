using System.Collections.ObjectModel;
using BankingApp.DTOs;
using BankingApp.Entities.Users;
using BankingApp.Services.RegistrationService;

namespace BankingApp.Services.SystemAdminService;

    public class SystemAdminService : ISystemAdminService
    {
        private readonly IRegistrationService _registrationService;

        public SystemAdminService(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public ObservableCollection<User> GetAllUsers()
        {
            return _registrationService.GetUsers();
        }

        public ObservableCollection<User> GetUsersByRole(UserRole role)
        {
            var users = _registrationService.GetUsers();
            return new ObservableCollection<User>(users.Where(u=>u.Role == role));
        }
        

        public void AssignRole(Guid userId, UserRole newRole)
        {
          var user = _registrationService.GetUserById(userId);
          if (user != null)
          {
              user.Role = newRole;
          }
        }

       
    }