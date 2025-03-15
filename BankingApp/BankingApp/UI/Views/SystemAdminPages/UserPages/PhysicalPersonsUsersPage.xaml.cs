using BankingApp.UI.ViewModels.RoleBasedModels;

namespace BankingApp.UI.Views.SystemAdminPages.UserPages;

public partial class PhysicalPersonsUsersPage : ContentPage
{
    public PhysicalPersonsUsersPage(SystemAdminViewModel systemAdminViewModel)
    {
        InitializeComponent();
        try
        {
            InitializeComponent();
            BindingContext = systemAdminViewModel;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing AdminPage: {ex.Message}");
        }
    }
}