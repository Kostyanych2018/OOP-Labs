using BankingApp.UI.Views.DashboardPages;
using BankingApp.UI.Views.DashboardPages.ClientPages;

namespace BankingApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        CurrentItem = RegistrationPage;
    }
    
}