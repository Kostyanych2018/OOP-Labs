namespace BankingApp.Entities.Customers;

public class PhysicalPerson : Customer
{
    public string? FullName { get; set; }
    public string? PassportSeries {get; set;}
    public string? PassportNumber { get; set; }
    public string? PhoneNumber { get; set; }
}