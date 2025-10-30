using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CheckLibrary.Services
{
    public class LibraryService : IBaseRegister<Library>
    {
        private readonly CheckLibraryDbContext _context;

        public LibraryService(CheckLibraryDbContext context)
        {
            _context = context;
        }
        public LibraryService() { }
        public async Task<List<Library>> FindAllAsync()
        {
            return await _context.Library.OrderBy(item => item.Title).ToListAsync();
        }
        public async Task<Library> FindByIdAsync(int id)
        {
            return await _context.Library.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task InsertAsync(Library library)
        {
            _context.Add(library);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Library library)
        {
            bool hasAny = _context.Library.Any(x => x.Id == library.Id);
            if (!hasAny) { throw new NotFoundException("Id Not Found"); }

            try
            {
                _context.Library.Update(library);
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
                var libraryDelete = await _context.Library.Where(library => library.Id == id).ToListAsync();

                _context.Library.Remove(libraryDelete.First());

                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }
        public List<Library> FindByWord(string word)
        {
            try
            {
                List<Library> wordFind = _context.Library
                    .Where(Library => EF.Functions.Like(Library.Title, String.Format("%{0}%", word))).ToList();
                return wordFind;
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }
    }
}