using Microsoft.Win32.SafeHandles;

namespace Lab1.Entities.Users;

public class Client : AbstractUser
{
    public override string Role => "Client";
    public string? FullName { get; set; }
    public string? PassportNumber { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

    public Client(string username, string password)
    {
        Username = username;
        Password= password;
    }
    
}