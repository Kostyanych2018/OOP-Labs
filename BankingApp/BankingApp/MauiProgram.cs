using System.Collections.ObjectModel;
using BankingApp.Entities.Customers;
using BankingApp.Entities.Users;
using BankingApp.Services.AccountService;
using BankingApp.Services.AdminPanelService;
using BankingApp.Services.AuthorizationService;
using BankingApp.Services.NavigationService;
using BankingApp.Services.RegistrationService;
using BankingApp.UI.ViewModels;
using BankingApp.UI.ViewModels.RoleBasedModels;
using BankingApp.UI.Views;
using BankingApp.UI.Views.DashboardPages.ClientPages;
using BankingApp.UI.Views.ServicePages;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

// using Xamarin.KotlinX.Coroutines;

namespace BankingApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IRegistrationService, RegistrationService>();
        builder.Services.AddSingleton<IAdminPanelService, AdminPanelService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IAccountService,AccountService>();
        
        builder.Services.AddTransient<RegistrationViewModel>();
        builder.Services.AddTransient<AdminPanelViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ClientProfileViewModel>();
        builder.Services.AddTransient<ClientAccountsViewModel>();

        builder.Services.AddTransient<RegistrationPage>();
        builder.Services.AddTransient<RequestsPage>();
        builder.Services.AddTransient<UsersPage>();
        builder.Services.AddTransient<LoginPage>();
        
        builder.Services.AddTransient<ClientProfilePage>();
        builder.Services.AddTransient<ClientAccountsPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}