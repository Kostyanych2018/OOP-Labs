using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.UI.ViewModels;

namespace BankingApp.UI.Views;

public partial class UsersPage : ContentPage
{
    public UsersPage(AdminPanelViewModel adminPanelViewModel)
    {
        try
        {
            InitializeComponent();
            BindingContext = adminPanelViewModel;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing AdminPage: {ex.Message}");
        }
    }
}