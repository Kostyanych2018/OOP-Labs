namespace BankingApp.Entities.Banking;

public class Loan
{
    public Guid LoanId { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal TotalPayment { get; set; }
    public decimal RemainingAmount { get; set; }
    public int TermInMonths { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public Guid? ApprovedUserId { get; set; }
    public LoanType Type { get; set; }
    public LoanStatus Status { get; set; }
}

public enum LoanType
{
    PersonalLoan,//кредит
    Installment//рассрочка
}

public enum LoanStatus
{
    Pending,
    Approved,
    Rejected,
    Active,
    Completed,
    Default
}

