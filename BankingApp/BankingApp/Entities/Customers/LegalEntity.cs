namespace BankingApp.Entities.Customers;
//юридическое лицо - будут банки и предприятия
public class LegalEntity: Customer
{
    public string? LegalName { get; set; }
    public string? LegalAddress { get; set; }
    public string? TaxIdNumber { get; set; }
    public string? LegalEntityType { get; set; }
}
