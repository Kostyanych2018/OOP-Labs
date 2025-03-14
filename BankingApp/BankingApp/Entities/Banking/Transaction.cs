namespace BankingApp.Entities.Banking;

public class Transaction
{
    public Guid TransactionId { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid? DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TimeStamp { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public string Description { get; set; } = "";
}

public enum TransactionType
{
    Deposit,
    Withdrawal,
    Transfer,
    Interest,
    Fee
}

public enum TransactionStatus
{
    Completed,
    Canceled,
    Pending,
    Failed
}