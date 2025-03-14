using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BankingApp.DTOs;
using BankingApp.Entities.Users;
using BankingApp.Services.AdminPanelService;
using BankingApp.Services.RegistrationService;

namespace BankingApp.UI.ViewModels;

public class AdminPanelViewModel : INotifyPropertyChanged
{
    private readonly IAdminPanelService _adminPanelService;
    private readonly IRegistrationService _registrationService;
    
    private User? _selectedUser;
    private RegistrationRequest? _selectedRequest;
    private UserRole _selectedUserRole;
    private bool _isRefreshing;

    public ObservableCollection<User> Users { get; set; }
    public ObservableCollection<RegistrationRequest> PendingRequests { get; set; }
    public List<UserRole> AvailableRoles { get; }

    public ICommand AssignRoleCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ApproveCommand { get; }
    public ICommand RejectCommand { get; }

    public AdminPanelViewModel(IAdminPanelService adminPanelService,IRegistrationService registrationService)
    {
        _adminPanelService = adminPanelService;
        _registrationService = registrationService;
        
        AvailableRoles = Enum.GetValues<UserRole>().ToList();
        Users = [];
        PendingRequests = [];
        
        AssignRoleCommand = new Command<User>(OnAssignRole,_=>CanAssignRole());
        RefreshCommand = new Command(async ()=>await LoadDataAsync());
        ApproveCommand = new Command<RegistrationRequest>(ApproveRegistration,
            request => request != null && request.Status == RegistrationStatus.Pending);
        RejectCommand = new Command<RegistrationRequest>(RejectRegistration,
            request => request != null && request.Status == RegistrationStatus.Pending);
        LoadDataAsync();
    }

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (_selectedUser == value) return;
            _selectedUser = value;
            if (_selectedUser != null)
            {
                SelectedUserRole = _selectedUser.Role;
            }

            OnPropertyChanged();
            ((Command<User>)AssignRoleCommand).ChangeCanExecute();
        }
    }

    public UserRole SelectedUserRole
    {
        get => _selectedUserRole;
        set
        {
            if (_selectedUserRole == value) return;
            _selectedUserRole = value;
            OnPropertyChanged();
            ((Command<User>)AssignRoleCommand).ChangeCanExecute();
        }
    }
    
    public RegistrationRequest? SelectedRequest
    {
        get => _selectedRequest;
        set
        {
            if (_selectedRequest == value) return;
            _selectedRequest = value;
            OnPropertyChanged();
            ((Command)ApproveCommand)?.ChangeCanExecute();
            ((Command)RejectCommand)?.ChangeCanExecute();
        }
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            if (_isRefreshing == value) return;
            _isRefreshing = value;
            OnPropertyChanged();
        }
    }

    public bool CanAssignRole()=>  _selectedUser != null && _selectedUser.Role != _selectedUserRole;
    public async void OnAssignRole(object obj)
    {
        if (obj is User user)
        {
            try
            {
                _adminPanelService.AssignRole(user.UserId, _selectedUserRole);
                await Shell.Current.DisplayAlert("Уведомление", "Роль успешно изменена", "ОК");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Не удалось изменить роль: {ex.Message}", "ОК");
            }
        }
    }

    public async Task LoadDataAsync()
    {
        try
        {
            IsRefreshing = true;
            Users=_registrationService.GetUsers();
            PendingRequests= _registrationService.GetRegistrationRequests();
            OnPropertyChanged(nameof(Users));
            OnPropertyChanged(nameof(PendingRequests));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить данные: {ex.Message}", "OK");
        }
        finally
        {
            IsRefreshing = false;
        }
    }
    
    private void ApproveRegistration(RegistrationRequest request)
    {
        try
        {
            _registrationService.ApproveRegistration(request);
            LoadDataAsync();
            Shell.Current.DisplayAlert("Уведомление", "Регистрация одобрена", "OK");
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить заявки: {ex.Message}", "OK");
        }
    }

    private void RejectRegistration(RegistrationRequest request)
    {
        try
        {
            _registrationService.RejectRegistration(request);
            LoadDataAsync();
            Shell.Current.DisplayAlert("Уведомление", "Регистрация отклонена", "ОК");
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить заявки: {ex.Message}", "OK");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}