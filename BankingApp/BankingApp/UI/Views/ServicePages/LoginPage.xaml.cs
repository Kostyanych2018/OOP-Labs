using BankingApp.UI.ViewModels;

namespace BankingApp.UI.Views.ServicePages;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        BindingContext=loginViewModel;
    }
}