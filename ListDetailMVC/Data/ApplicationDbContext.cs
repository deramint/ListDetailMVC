using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ListDetailMVC.Models;

namespace ListDetailMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ListDetailMVC.Models.Book> Book { get; set; }
        public DbSet<ListDetailMVC.Models.Author> Author { get; set; }
        public DbSet<ListDetailMVC.Models.Publisher> Publisher { get; set; }
    }
}