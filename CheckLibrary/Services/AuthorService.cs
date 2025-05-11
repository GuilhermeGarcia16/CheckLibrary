using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CheckLibrary.Services
{
    public class AuthorService
    {
        private readonly CheckLibraryDbContext _context;

        public AuthorService(CheckLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> FindAllAsync()
        {
           return await _context.Author.OrderBy(item => item.Name).ToListAsync();
        }

        public async Task<Author> FindByAsync(int id)
        {
            return await _context.Author.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task InsertAsync(Author author)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            bool hasAny = _context.Author.Any(x => x.Id == author.Id);
            if(!hasAny) { throw new NotFoundException("Id Not Found");};
            
            try
            {
                _context.Author.Update(author);
                await _context.SaveChangesAsync();
            }catch(DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
          
        }
    }
}
