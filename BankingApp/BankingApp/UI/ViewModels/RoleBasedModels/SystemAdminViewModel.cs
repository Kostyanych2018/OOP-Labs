using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BankingApp.DTOs;
using BankingApp.Entities.Users;
using BankingApp.Services.RegistrationService;
using BankingApp.Services.SystemAdminService;

namespace BankingApp.UI.ViewModels.RoleBasedModels;

public class SystemAdminViewModel : INotifyPropertyChanged
{
    private readonly ISystemAdminService _systemAdminService;
    private readonly IRegistrationService _registrationService;
    
    private User? _selectedUser;
    private RegistrationRequest? _selectedRequest;
    private bool _isRefreshing;

    public ObservableCollection<User> Users { get; set; }
    public ObservableCollection<RegistrationRequest> PendingRequests { get; set; }
    public List<UserRole> AvailableRoles { get; }
    
    public ICommand RefreshCommand { get; }
    public ICommand ApproveCommand { get; }
    public ICommand RejectCommand { get; }

    public SystemAdminViewModel(ISystemAdminService systemAdminService,IRegistrationService registrationService)
    {
        _systemAdminService = systemAdminService;
        _registrationService = registrationService;
        
        AvailableRoles = Enum.GetValues<UserRole>().ToList();
        Users = [];
        PendingRequests = [];
        
        RefreshCommand = new Command(LoadData);
        ApproveCommand = new Command<RegistrationRequest>(ApproveRegistration,
            request => request != null && request.Status == RegistrationStatus.Pending);
        RejectCommand = new Command<RegistrationRequest>(RejectRegistration,
            request => request != null && request.Status == RegistrationStatus.Pending);
        LoadData();
    }

    public ObservableCollection<User> Banks => new ObservableCollection<User>(Users.Where(u => u.CustomerType
                                                                                                    == CustomerType.Bank));
    public ObservableCollection<User> Enterprises => new ObservableCollection<User>(Users.Where(u => u.CustomerType
                                                                                                    == CustomerType.Enterprise));
    public ObservableCollection<User> PhysicalPersons => new ObservableCollection<User>(Users.Where(u => u.CustomerType
                                                                                                    == CustomerType.PhysicalPerson));

    public void LoadData()
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
             Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить данные: {ex.Message}", "OK");
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
            LoadData();
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
            LoadData();
            Shell.Current.DisplayAlert("Уведомление", "Регистрация отклонена", "ОК");
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить заявки: {ex.Message}", "OK");
        }
    }
    
    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (_selectedUser == value) return;
            _selectedUser = value;
            OnPropertyChanged();
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}