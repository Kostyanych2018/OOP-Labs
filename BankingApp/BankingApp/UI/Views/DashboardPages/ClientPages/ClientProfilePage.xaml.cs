using BankingApp.UI.ViewModels.RoleBasedModels;

namespace BankingApp.UI.Views.DashboardPages.ClientPages;

public partial class ClientProfilePage :  ContentPage
{
    public ClientProfilePage(ProfileViewModel profile)
    {
        InitializeComponent();
        BindingContext = profile;
    }
}