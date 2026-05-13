
using Microsoft.EntityFrameworkCore;
using PersonApi.Models;

namespace PersonApi.Context;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Person> People {get; set;}
    public DbSet<Adress> Adresses {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //relation person to adres
        modelBuilder.Entity<Person>()
        .HasOne(p => p.Adress)
        .WithMany(a => a.People)
        .HasForeignKey(p => p.AdressId);
        
        //declare a adress id
        string adressId = Guid.NewGuid().ToString()[..6];

        modelBuilder.Entity<Adress>().HasData([
            new("Street 1","12345","City") {Id = adressId}
        ]);


         modelBuilder.Entity<Person>().HasData([
            new ("John Doe", 30, false) {Id = "PERS01", AdressId = adressId},
            new ("Jane Smith", 25, true){Id = "PERS02", AdressId = adressId},
            new ("Alice Johnson", 40, true) {Id = "PERS03", AdressId = adressId}
        ]);

    base.OnModelCreating(modelBuilder);
    }
    
}