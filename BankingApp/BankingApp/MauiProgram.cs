using System.Collections.ObjectModel;
using BankingApp.Entities.Customers;
using BankingApp.Entities.Users;
using BankingApp.Services.AccountService;
using BankingApp.Services.AuthorizationService;
using BankingApp.Services.NavigationService;
using BankingApp.Services.RegistrationService;
using BankingApp.Services.SystemAdminService;
using BankingApp.UI.ViewModels;
using BankingApp.UI.ViewModels.RoleBasedModels;
using BankingApp.UI.Views;
using BankingApp.UI.Views.DashboardPages.ClientPages;
using BankingApp.UI.Views.ServicePages;
using BankingApp.UI.Views.SystemAdminPages.UserPages;
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
        builder.Services.AddSingleton<ISystemAdminService, SystemAdminService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IAccountService,AccountService>();
        
        builder.Services.AddTransient<RegistrationViewModel>();
        builder.Services.AddTransient<SystemAdminViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<ClientAccountsViewModel>();

        builder.Services.AddTransient<RegistrationPage>();
        builder.Services.AddTransient<RequestsPage>();
        builder.Services.AddTransient<LoginPage>();
        
        builder.Services.AddTransient<BankUsersPage>();
        builder.Services.AddTransient<EnterpriseUsersPage>();
        builder.Services.AddTransient<PhysicalPersonsUsersPage>();
        
        builder.Services.AddTransient<ClientProfilePage>();
        builder.Services.AddTransient<ClientAccountsPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}