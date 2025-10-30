using Microsoft.EntityFrameworkCore;
using CheckLibrary.Models;

namespace CheckLibrary.Data
{
    public class CheckLibraryDbContext : DbContext
    {
        public CheckLibraryDbContext(DbContextOptions<CheckLibraryDbContext> options) : base(options)
        { }

        public DbSet<Author> Author { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Book> Book { get; set; } = default!;
        public DbSet<Account> Account { get; set; } = default!;
        public DbSet<Library> Library { get; set; } = default!;
    }
}