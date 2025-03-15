using BankingApp.DTOs;
using BankingApp.Entities.Customers;
using BankingApp.Entities.Users;

namespace BankingApp.Services;

public static class DataGenerationService
{
    private static Random _random = new Random();

    public static List<Bank> GenerateBanks(int count)
    {
        var banks = new List<Bank>();
        banks.Add(new Bank()
        {
            LegalName = "Сбербанк",
            LegalAddress = "ул. Ленина, д. 1",
            TaxIdNumber = "111222333",
            LegalEntityType = "ООО",
            BankIdCode = "SB001",
            Email = "contact@sberbank.ru",   
            
        });
        banks.Add(new Bank()
        {
            LegalName = "ВТБ",
            LegalAddress = "пр. Мира, д. 5",
            TaxIdNumber = "444555666",
            LegalEntityType = "ОАО",
            BankIdCode = "VTB002",
            Email = "contact@vtb.ru",
        });
        for (int i = banks.Count; i < count; i++)
        {
            banks.Add(new Bank()
            {
                LegalName = $"Банк {i + 1}",
                LegalAddress = $"ул. Примерная, д. {_random.Next(1, 100)}",
                TaxIdNumber = _random.Next(100000000, 999999999).ToString(),
                LegalEntityType = (i % 2 == 0) ? "ООО" : "ОАО",
                BankIdCode = $"BANK{i + 1:000}",
                Email = $"bank{i + 1}@example.com"
            });
        }

        return banks;
    }

    public static List<Enterprise> GenerateEnterprises(int count)
    {
        var enterprises = new List<Enterprise>();
        enterprises.Add(new Enterprise()
        {
            LegalName = "Газпром",
            LegalAddress = "ул. Газетная, д. 10",
            TaxIdNumber = "987654321",
            LegalEntityType = "ОАО",
            IndustryType = "Энергетика",
            Email = "contact@gazprom.ru"
        });
        enterprises.Add(new Enterprise()
        {
            LegalName = "Роснефть",
            LegalAddress = "пр. Нефтяников, д. 15",
            TaxIdNumber = "123456789",
            LegalEntityType = "ПАО",
            IndustryType = "Нефть и газ",
            Email = "info@rosneft.ru"
        });
        for (int i = enterprises.Count; i < count; i++)
        {
            enterprises.Add(new Enterprise()
            {
                LegalName = $"Предприятие {i + 1}",
                LegalAddress = $"пр. Промышленный, д. {_random.Next(1, 100)}",
                TaxIdNumber = _random.Next(100000000, 999999999).ToString(),
                LegalEntityType = (i % 2 == 0) ? "ООО" : "ОАО",
                IndustryType = (i % 2 == 0) ? "Технологии" : "Производство",
                Email = $"enterprise{i + 1}@example.com"
            });
        }

        return enterprises;
    }

    public static List<PhysicalPerson> GeneratePhysicalPersons(int count)
    {
        var persons = new List<PhysicalPerson>();
        persons.Add(new PhysicalPerson()
        {
            FullName = "Иван Иванов",
            PassportSeries = "AB",
            PassportNumber = "123456",
            PhoneNumber = "+7-111-222-33-44",
            Email = "ivanov@example.com"
        });
        persons.Add(new PhysicalPerson()
        {
            FullName = "Петр Петров",
            PassportSeries = "PP",
            PassportNumber = "654321",
            PhoneNumber = "+7-555-666-77-88",
            Email = "petrov@example.com"
        });
        var firstNames = new List<string> { "Алексей", "Дмитрий", "Сергей", "Николай", "Михаил" };
        var lastNames = new List<string> { "Сидоров", "Козлов", "Морозов", "Смирнов", "Волков" };
        for (int i = persons.Count; i < count; i++)
        {
            var firstName = firstNames[_random.Next(0, firstNames.Count)];
            var lastName = lastNames[_random.Next(0, lastNames.Count)];
            persons.Add(new PhysicalPerson()
            {
                FullName = $"{firstName} {lastName}",
                PassportSeries = $"{firstName[0]}{lastName[0]}",
                PassportNumber = _random.Next(100000, 999999).ToString(),
                PhoneNumber = $"+7-{_random.Next(100, 999)}-{_random.Next(100, 999)}-{_random.Next(10, 99)}-{_random.Next(10, 99)}",
                Email = $"{firstName.ToLower()}.{lastName.ToLower()}@example.com"
            });
        }

        return persons;
    }
    public static List<User> GenerateUsers(List<Bank> banks, List<Enterprise> enterprises, List<PhysicalPerson> persons)
    {
        var users = new List<User>();
        
        foreach (var bank in banks)
        {
            users.Add(new User()
            {
                UserId = Guid.NewGuid(),
                Username = bank.LegalName?.Replace(" ", "").ToLower(),
                Password = "bank123",
                Customer = bank,
                CustomerType = CustomerType.Bank,
                Role = UserRole.BankAdmin
            });
        }
        
        foreach (var enterprise in enterprises)
        {
            users.Add(new User()
            {
                UserId = Guid.NewGuid(),
                Username = enterprise.LegalName?.Replace(" ", "").ToLower(),
                Password = "enterprise123",
                Customer = enterprise,
                CustomerType = CustomerType.Enterprise,
                Role = UserRole.EnterpriseAdmin
            });
        }
        
        foreach (var person in persons)
        {
            users.Add(new User()
            {
                UserId = Guid.NewGuid(),
                Username = person.FullName?.Replace(" ", "").ToLower(),
                Password = "user123",
                Customer = person,
                CustomerType = CustomerType.PhysicalPerson,
                Role = UserRole.Client
            });
        }

        return users;
    }
}
