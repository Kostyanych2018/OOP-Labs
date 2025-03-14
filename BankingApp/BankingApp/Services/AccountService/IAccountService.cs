using BankingApp.Entities.Banking;

// using Account = Android.Accounts.Account;

namespace BankingApp.Services.AccountService;

public interface IAccountService
{
    Task<List<Account>> GetAccounts(Guid userId);
    Task<Account> OpenAccount(Guid customerId, string bankIdCode, AccountType accountType);
    Task<bool> CloseAccount(Guid accountId);
    Task<bool> BlockAccount(Guid accountId);
    Task<bool> UnblockAccount(Guid accountId);

    Task<Transaction> Deposit(Guid accountId, decimal amount);
    Task<Transaction> Withdraw(Guid accountId, decimal amount);
    Task<Transaction> Transfer(Guid sourceAccountId, Guid destinationAccountId, decimal amount);
    Task<List<Transaction>> GetTransactions(Guid accountId);
}