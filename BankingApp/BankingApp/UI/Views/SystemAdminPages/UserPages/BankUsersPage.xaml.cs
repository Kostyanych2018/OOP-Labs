using BankingApp.UI.ViewModels.RoleBasedModels;

namespace BankingApp.UI.Views.SystemAdminPages.UserPages
{
    public partial class BankUsersPage : ContentPage
    {
        public BankUsersPage(SystemAdminViewModel systemAdminViewModel)
        {
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
}