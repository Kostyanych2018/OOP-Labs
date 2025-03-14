using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.Entities.Banking;
using BankingApp.UI.ViewModels.RoleBasedModels;

namespace BankingApp.UI.Views.DashboardPages.ClientPages;

public partial class ClientAccountsPage : ContentPage
{
    private readonly ClientAccountsViewModel _clientAccountsViewModel;
    
    public ClientAccountsPage(ClientAccountsViewModel clientAccountsViewModel)
    {
        InitializeComponent();
        _clientAccountsViewModel = clientAccountsViewModel;
        BindingContext = clientAccountsViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _clientAccountsViewModel.LoadAccounts();
    }

    private async void OnAccountOperationsClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var account = (Account)button.BindingContext;
        _clientAccountsViewModel.SelectedAccount = account;
        var modal = new AccountOperationsModal()
        {
            BindingContext = _clientAccountsViewModel
        };
        await Navigation.PushModalAsync(modal);
    }
}