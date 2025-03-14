using BankingApp.Entities.Users;
using BankingApp.UI.Views.DashboardPages;
using BankingApp.UI.Views.DashboardPages.ClientPages;

namespace BankingApp.Services.NavigationService;

public class NavigationService : INavigationService
{
    

    public async Task NavigateToDashboardAsync(User user)
    {
        if (user.Role is UserRole.Client)
            await Shell.Current.GoToAsync("//Client/Accounts");
        //доделать для остальных
    }

    public async Task GoBackAsync()
    {
        await Shell.Current.Navigation.PopAsync();
    }

    public async Task GoBackToRootAsync()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}