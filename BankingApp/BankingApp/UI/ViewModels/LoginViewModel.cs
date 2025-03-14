using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BankingApp.DTOs;
using BankingApp.Services.AuthorizationService;
using BankingApp.Services.NavigationService;

namespace BankingApp.UI.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    private string _username = string.Empty;
    private string _password = string.Empty;
    private bool _isLoading;
    private string _errorMessage = string.Empty;
    private string _successMessage = string.Empty;

    public ICommand LoginCommand { get; set; }

    public LoginViewModel(IAuthService authService,INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        LoginCommand = new Command( async ()=>await ExecuteLoginCommand(), ()=> CanExecuteLoginCommand);
    }

    private bool CanLogin()
    {
        return !string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password);
    }

    public  bool CanExecuteLoginCommand =>CanLogin(); 
    private async Task ExecuteLoginCommand()
    {
        try
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            var user = await _authService.LoginAsync(_username, _password);
            if (user!=null)
            {
                SuccessMessage = "Вход выполнен успешно!";
                await _navigationService.NavigateToDashboardAsync(user);
            }
            else
            {
                ErrorMessage = "Неверное имя пользователя или пароль";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка: {ex.Message}";
        }
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public string SuccessMessage
    {
        get => _successMessage;
        set => SetProperty(ref _successMessage, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        ((Command)LoginCommand).ChangeCanExecute();
        return true;
    }
}