using BankingApp.UI.ViewModels.RoleBasedModels;

namespace BankingApp.UI.Views.DashboardPages.ClientPages;

public partial class ClientProfilePage :  ContentPage
{
    public ClientProfilePage(ClientProfileViewModel clientProfile)
    {
        InitializeComponent();
        BindingContext = clientProfile;
    }
}