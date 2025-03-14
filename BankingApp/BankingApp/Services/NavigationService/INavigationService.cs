using BankingApp.Entities.Users;

namespace BankingApp.Services.NavigationService;

public interface INavigationService
{
    Task NavigateToDashboardAsync(User user);
    Task GoBackAsync();
    Task GoBackToRootAsync();
}