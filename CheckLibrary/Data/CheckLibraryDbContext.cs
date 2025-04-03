using Microsoft.EntityFrameworkCore;
using CheckLibrary.Models;

namespace CheckLibrary.Data
{
    public class CheckLibraryDbContext : DbContext
    {
        public CheckLibraryDbContext(DbContextOptions<CheckLibraryDbContext> options) : base(options)
        { }

        public DbSet<Author> Author { get; set; } = default!;
    }
}
