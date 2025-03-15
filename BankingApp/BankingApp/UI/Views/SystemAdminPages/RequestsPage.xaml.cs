using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Services.RegistrationService;
using BankingApp.UI.ViewModels;
using BankingApp.UI.ViewModels.RoleBasedModels;

namespace BankingApp.UI.Views;

public partial class RequestsPage : ContentPage
{
    public RequestsPage(SystemAdminViewModel systemAdminViewModel)
    {
        InitializeComponent();
        BindingContext = systemAdminViewModel;
    }
}