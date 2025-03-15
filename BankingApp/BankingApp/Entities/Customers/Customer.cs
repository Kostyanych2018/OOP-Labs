namespace BankingApp.Entities.Customers;
//работаем c клиентами, используем абстрактный класс для дальнейшей удобной работы
public abstract class Customer
{
     public string? Email { get; set; }
     public abstract string? Name { get; set; }
}
// у каждого пользователя (предприятия и тд будет id, email)

// Customer (abstract)
// └── PhysicalPerson └── LegalEntity (abstract)
//                     └── Enterprise └── Bank
//    