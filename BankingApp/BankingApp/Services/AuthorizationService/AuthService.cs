using BankingApp.Entities.Users;
using BankingApp.Services.RegistrationService;

namespace BankingApp.Services.AuthorizationService;

public class AuthService : IAuthService
{
    private readonly IRegistrationService _registrationService;
    private User? _currentUser;
    
    public User? CurrentUser => _currentUser;
    public UserRole? CurrentUserRole => _currentUser?.Role;

    public AuthService(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        return await Task.Run(() =>
        {
            var users = _registrationService.GetUsers();
            var user = users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password.Equals(password));
            if (user != null)
            {
                _currentUser = user;
            }

            return _currentUser;
        });
    }
    
   
    public void Logout()
    {
        _currentUser = null;
    }
}

public class UserAuthEventArgs : EventArgs
{
    public User? User { get; set; }
}