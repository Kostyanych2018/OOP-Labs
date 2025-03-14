namespace BankingApp.Entities.Banking;

public class SalaryProject
{
    public Guid ProjectId { get; set; }
    public Guid EnterpriseId { get; set; }
    public Guid BankId { get; set; }
    public DateTime CreationDate { get; set; }
    public SalaryProjectStatus Status { get; set; }
    public List<SalaryProjectEmployee> Employees { get; set; } = [];
    public Guid? ApprovedByOperatorId { get; set; }
    public DateTime ApprovalDate { get; set; }
}

public enum SalaryProjectStatus
{
    Pending,
    Approved,
    Active,
    Terminated,
    Suspended
}

public class SalaryProjectEmployee
{
    public Guid SalaryProjectId { get; set; }
    public Guid AccountId { get; set; }
    public decimal SalaryAmount { get; set; }
    public DateTime JoinDate { get; set; }
}