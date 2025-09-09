using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services;

namespace CheckLibrary.Application
{
    public class LoginManagement
    {
        private readonly AccountService _accountService;
        public LoginManagement(AccountService accountService) 
        {
            _accountService = accountService;
        }
        public async Task<StatusMessage<Account>> Login(Account account)
        {
            Account accountFind = await _accountService.FindAccountByEmailAsync(account.Email);
            string message = "";
            if (accountFind is null) 
            {
                message = "Account is NOT exists.";
                return new StatusMessage<Account>(accountFind, false, message); 
            }

            Boolean accountLogon = PasswordService.VerifyPassword(account.Password, accountFind.Password);
            if(!accountLogon)
            {
                message = "Password is wrong.";
                return new StatusMessage<Account>(accountFind, accountLogon, message);
            }

            return new StatusMessage<Account>(accountFind, accountLogon, String.Empty);
        }

        public async Task<StatusMessage<Account>> Create(Account account)
        {
            string message = "Account Created.";
            string passwordEncrypted = PasswordService.CriptographyPassword(account.Password);
            account.Password = passwordEncrypted;
            account.ConfirmPassword = passwordEncrypted;

            await _accountService.InsertAsync(account);

            return new StatusMessage<Account>(account, true, message);
        }
    }
}