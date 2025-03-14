namespace BankingApp.Entities.Banking;

public class Account
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public string BankIdCode { get; set; } = "";
    public decimal Balance { get; set; } = 0;
    public string AccountNumber { get; set; } = "";
    public AccountType Type { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.Active;
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public decimal InterestRate { get; set; }//процентная ставка  для накомительных счетов
}

public enum AccountStatus
{
    Active,
    Blocked,
    Closed
}

public enum AccountType
{
    Checking,
    Savings,
    Salary
}