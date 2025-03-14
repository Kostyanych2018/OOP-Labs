using System.Collections.ObjectModel;
using BankingApp.Entities.Banking;

namespace BankingApp.Services.AccountService;

public class AccountService:IAccountService
{
    private readonly ObservableCollection<Account?> _accounts = [];
    private readonly ObservableCollection<Transaction> _transactions = [];

    public Task<Account> OpenAccount(Guid userId, string bankIdCode, AccountType type)
    {
        var account = new Account
        {
            AccountId = Guid.NewGuid(),
            UserId = userId,
            BankIdCode = bankIdCode,
            AccountNumber = GenerateAccountNumber(),
            Type = type,
            Balance = 0,
            Status = AccountStatus.Active,
            OpenDate = DateTime.Now,
            InterestRate = type == AccountType.Savings ? 0.05m : 0m
        };
        _accounts.Add(account);
        return Task.FromResult(account);
    }
    private string GenerateAccountNumber()
    {
        return new Random().Next((int)1e8, (int)9e8).ToString();
    }
    public async Task<bool> CloseAccount(Guid accountId)
    {
        var account = _accounts.FirstOrDefault(a => a != null && a.AccountId == accountId);
        if (account != null)
        {
            account.Status = AccountStatus.Closed;
            account.CloseDate = DateTime.Now;
            return true;
        }
        return false;
    }

    public Task<List<Account>> GetAccounts(Guid userId)
    {
        return Task.FromResult(_accounts.Where(a =>  a.UserId==userId).ToList());
    }
    

    public Task<bool> BlockAccount(Guid accountId)
    {
        var account = _accounts.FirstOrDefault(a => a != null && a.AccountId == accountId);
        if (account == null || account.Status != AccountStatus.Active)
            return Task.FromResult(false);
        account.Status = AccountStatus.Blocked;
        return Task.FromResult(true);
    }

    public Task<bool> UnblockAccount(Guid accountId)
    {
        var account = _accounts.FirstOrDefault(a => a != null && a.AccountId == accountId);
        if (account == null || account.Status != AccountStatus.Blocked)
            return Task.FromResult(false);
        account.Status = AccountStatus.Active;
        return Task.FromResult(true);
    }

    public Task<Transaction> Deposit(Guid accountId, decimal amount)
    {
        var account = _accounts.FirstOrDefault(a => a != null && a.AccountId == accountId);
        if(account == null || account.Status != AccountStatus.Active)
            throw new InvalidOperationException($"Невозможно внести депозит на счет {accountId}: счет не найден или неактивен");
        if (amount <= 0)
            throw new ArgumentException("Сумма депозита должна быть положительной");
        var transaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            SourceAccountId = accountId,
            DestinationAccountId = null,
            Amount = amount,
            Type = TransactionType.Deposit,
            Status = TransactionStatus.Completed,
            TimeStamp = DateTime.Now,
            Description = "Депозит"
        };
        account.Balance += amount;
        _transactions.Add(transaction);
        return Task.FromResult(transaction);
    }

    public Task<Transaction> Withdraw(Guid accountId, decimal amount)
    {   
        var account = _accounts.FirstOrDefault(a => a != null && a.AccountId == accountId);
        if(account == null || account.Status != AccountStatus.Active)
            throw new InvalidOperationException($"Невозможно снять деньги со счета {accountId}: счет не найден или неактивен");
        if (amount <= 0)
            throw new ArgumentException("Сумма вывода должна быть положительной");
        if (account.Balance < amount)
            throw new InvalidOperationException($"Недостаточно средств");
        var transaction = new Transaction()
        {
            TransactionId = Guid.NewGuid(),
            SourceAccountId = accountId,
            DestinationAccountId = null,
            Amount = amount,
            Type = TransactionType.Withdrawal,
            Status = TransactionStatus.Completed,
            TimeStamp = DateTime.Now,
            Description = "Вывод"
        };
        account.Balance -= amount;
        _transactions.Add(transaction);
        return Task.FromResult(transaction);
    }
    
    public Task<Transaction> Transfer(Guid sourceAccountId, Guid destinationAccountId, decimal amount)
    {
        var sourceAccount = _accounts.FirstOrDefault(a => a != null && a.AccountId == sourceAccountId);
        var destinationAccount = _accounts.FirstOrDefault(a => a != null && a.AccountId == destinationAccountId);
        if (sourceAccount == null || sourceAccount.Status != AccountStatus.Active)
            throw new InvalidOperationException($"Нельзя сделать перевод со счета {sourceAccountId}" +
                                                $": счет не найден или неактивен ");
        if(destinationAccount == null || destinationAccount.Status != AccountStatus.Active)
            throw new InvalidOperationException($"Нельзя сделать перевод на счет {destinationAccountId}" +
                                                $": счет не найден или неактивен ");
        if (amount <= 0)
            throw new ArgumentException("Сумма перевода должна быть положительной");
        if (sourceAccount.Balance < amount)
            throw new InvalidOperationException("Недостаточно средств");
        var transaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            SourceAccountId = sourceAccountId,
            DestinationAccountId = destinationAccountId,
            Amount = amount,
            Type = TransactionType.Transfer,
            Status = TransactionStatus.Completed,
            TimeStamp = DateTime.Now,
            Description = "Перевод"
        };
        sourceAccount.Balance -= amount;
        destinationAccount.Balance += amount;
        _transactions.Add(transaction);
        return Task.FromResult(transaction);
    }

    public Task<List<Transaction>> GetTransactions(Guid accountId)
    {
        var accountTransactions=_transactions.Where(t=>t.SourceAccountId==accountId ||
                                                       t.DestinationAccountId==accountId).ToList();
        return Task.FromResult(accountTransactions);
    }
}