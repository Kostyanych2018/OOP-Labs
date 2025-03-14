using BankingApp.UI.ViewModels;

namespace BankingApp.UI.Views.ServicePages;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage(RegistrationViewModel registrationViewModel)
    {
        InitializeComponent();
        BindingContext = registrationViewModel;
    }
}