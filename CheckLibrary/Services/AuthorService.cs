using CheckLibrary.Data;
using CheckLibrary.Models;

namespace CheckLibrary.Services
{
    public class AuthorService
    {
        private readonly CheckLibraryDbContext _context;

        public AuthorService(CheckLibraryDbContext context)
        {
            _context = context;
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
