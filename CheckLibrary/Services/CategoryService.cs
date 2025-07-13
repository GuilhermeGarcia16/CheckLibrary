using CheckLibrary.Data;
using CheckLibrary.Models;
using CheckLibrary.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CheckLibrary.Services
{
    public class CategoryService : IBaseRegister<Category>
    {
        private readonly CheckLibraryDbContext _context;

        public CategoryService(CheckLibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> FindAllAsync()
        {
            return await _context.Category.OrderBy(item => item.Description).ToListAsync();
        }

        public async Task<Category> FindByAsync(int id)
        {
            return await _context.Category.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task InsertAsync(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            bool hasAny = _context.Category.Any(x => x.Id == category.Id);
            if (!hasAny) { throw new NotFoundException("Id Not Found"); }

            try
            {
                _context.Category.Update(category);
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
                var authorDelete = await _context.Category.Where(category => category.Id == id).ToListAsync();

                _context.Category.Remove(authorDelete.First());

                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException ex)
            {
                throw new DBConcurrencyException(ex.Message);
            }
        }
    }
}