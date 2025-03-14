using System.ComponentModel;
using System.Runtime.CompilerServices;
using BankingApp.Entities.Users;

namespace BankingApp.DTOs;

public class RegistrationRequest:INotifyPropertyChanged
{
    public Guid RequestId { get; set; } = Guid.NewGuid();
    // public DateTime CreatedAt { get; set; } = DateTime.Now;
    private RegistrationStatus _status { get; set; } = RegistrationStatus.Pending;

    //общие данные для всех учетных записей
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public CustomerType CustomerType { get; set; }  

    //данные для физлица
    public string? FullName { get; set; }
    public string? PassportSeries { get; set; }
    public string? PassportNumber { get; set; }
    public string? PhoneNumber { get; set; }

    //данные для юр лица
    public string? LegalName { get; set; }
    public string? LegalAddress { get; set; }
    public string? TaxIdNumber { get; set; }
    public string? LegalEntityType { get; set; }

    public string? BankIdCode { get; set; }

    public string? IndustryType { get; set; }

    public RegistrationStatus Status
    {
        get => _status;
        set
        {
            if (_status == value) return;
            _status = value;
            OnPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

