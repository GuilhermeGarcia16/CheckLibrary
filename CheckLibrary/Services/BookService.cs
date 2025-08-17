using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CheckLibrary.Services
{
    public class BookService : IBaseRegister<Book>
    {
        private readonly CheckLibraryDbContext _context;

        public BookService(CheckLibraryDbContext context)
        {
            _context = context;
        }

        public BookService() { }

        public async Task<List<Book>> FindAllAsync()
        {
            return await _context.Book.OrderBy(item => item.Title)
                                      .ThenBy(item => item.CategoryID)
                                      .ThenBy(item => item.AuthorID)
                                      .ToListAsync();
        }

        public async Task<Book> FindByIdAsync(int id)
        {
            return await _context.Book.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task InsertAsync(Book Book)
        {
            _context.Add(Book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book Book)
        {
            bool hasAny = _context.Book.Any(x => x.Id == Book.Id);
            if (!hasAny) { throw new NotFoundException("Id Not Found"); }

            try
            {
                _context.Book.Update(Book);
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var authorDelete = await _context.Book.Where(Book => Book.Id == id).ToListAsync();

                _context.Book.Remove(authorDelete.First());

                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }

        public List<Book> FindByWord(String word)
        {
            try
            {
                List<Book> wordFind = _context.Book.Where(Book =>EF.Functions.Like(Book.Title, String.Format("%{0}%",word))).ToList();
                return wordFind;
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }
    }
}