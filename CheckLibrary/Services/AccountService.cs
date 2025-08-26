using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CheckLibrary.Services
{
    public class AccountService : IBaseRegister<Account>
    {
        private readonly CheckLibraryDbContext _context;
        public AccountService(CheckLibraryDbContext context)
        {
            _context = context;
        }
        public AccountService() { }
        public  async Task<List<Account>> FindAllAsync()
        {
            return await _context.Account.OrderBy(item => item.Email).ToListAsync();
        }
        public  async Task InsertAsync(Account account)
        {
            _context.Add(account);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Account account)
        {
            bool hasAny = _context.Account.Any(x => x.Email == account.Email);
            if (!hasAny) { throw new NotFoundException("Email Not Found"); }

            try
            {
                _context.Account.Update(account);
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }
        public async Task DeleteAsync(string email)
        {
            try
            {
                var accountDelete = await _context.Account.Where(account => account.Email.Equals(email)).ToListAsync();

                _context.Account.Remove(accountDelete.First());

                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }
        public List<Account> FindByWord(string word)
        {
            try
            {
                List<Account> wordFind = _context.Account.Where(Account => EF.Functions.Like(Account.Email, String.Format("%{0}%", word))).ToList();
                return wordFind;
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Account FindAccount(string email, string password = "")
        {
            try
            {
               return _context.Account.FirstOrDefault(item=> item.Email.Equals(email));
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }
    }
}