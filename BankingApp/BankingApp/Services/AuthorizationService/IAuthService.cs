using BankingApp.Entities.Users;

namespace BankingApp.Services.AuthorizationService;

public interface IAuthService
{
    Task<User?> LoginAsync(string username, string password);
    void Logout();
    User? CurrentUser { get; }
}
