using System.ComponentModel;
using System.Runtime.CompilerServices;
using BankingApp.DTOs;

namespace BankingApp.Entities.Users;
using BankingApp.Entities.Customers;

// данная сущность представляет учетную запись для авторизации и регистрации
public class User:INotifyPropertyChanged
{
    public string? Username { get; set; }
    public string? Password { get; set; }    
    public Guid UserId { get; set; }
    public CustomerType? CustomerType { get; set; }
    public Customer? Customer { get; set; }
    private UserRole _role;
    public UserRole Role
    {
        get => _role;
        set
        {
            if (_role == value) return;
            _role = value;
            OnPropertyChanged();
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) 
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
