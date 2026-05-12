
using Microsoft.EntityFrameworkCore;
using PersonApi.Models;

namespace PersonApi.Context;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Person> People {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.Entity<Person>().HasData([
            new ("John Doe", 30, false) {Id = "PERS01"},
            new ("Jane Smith", 25, true){Id = "PERS02"},
            new ("Alice Johnson", 40, true) {Id = "PERS03"}
        ]);
    }
}