using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.UI.Views.DashboardPages.ClientPages;

public partial class AccountOperationsModal : ContentPage
{
    public AccountOperationsModal()
    {
        InitializeComponent();
    }

    private async void  OnCloseClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}