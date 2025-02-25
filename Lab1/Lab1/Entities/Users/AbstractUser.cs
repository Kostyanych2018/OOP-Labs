namespace Lab1.Entities.Users;
//пользователь - авторизация и роли
public abstract class AbstractUser
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public abstract string Role { get; }
}