using Lab1.Entities.Users;

namespace Lab1.Services;
//управление заявками на регистрацию клиентов
public class RegistrationService
{
     private readonly List<Client> _clients = [];

     public void Register(string username,string password)
     {
         if (_clients.Any(c => c.Username == username))
         {
             throw new ArgumentException("Username is already taken");
         }
         var newClient = new Client(username, password);
         _clients.Add(newClient);
     }
}