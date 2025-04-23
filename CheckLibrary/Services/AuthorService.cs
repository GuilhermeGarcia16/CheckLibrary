using CheckLibrary.Data;
using CheckLibrary.Models;
using Microsoft.EntityFrameworkCore;

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
            
        }

        public async Task DeleteAsync(int id)
        {
          
        }
    }
}
