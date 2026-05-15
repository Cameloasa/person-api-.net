
using Microsoft.EntityFrameworkCore;
using PersonApi.Models;

namespace PersonApi.Context;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Person> People {get; set;}
    public DbSet<Adress> Adresses {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        //declare a adress id
        string adressId1 = Guid.NewGuid().ToString()[..6];
        string adressId2 = Guid.NewGuid().ToString()[..6];

        modelBuilder.Entity<Adress>().HasData([
            new("Street 1","12345","City") {Id = adressId1},
            new("New Street 1","54321","AnyCity") {Id = adressId2},
        ]);


         modelBuilder.Entity<Person>().HasData([
            new ("John Doe", 30, false) {Id = "PERS01"},
            new ("Jane Smith", 25, true){Id = "PERS02"},
            new ("Alice Johnson", 40, true) {Id = "PERS03"}
        ]);

    //relation person to adres
        modelBuilder.Entity<Person>()
        .HasMany(p => p.Adresses)
        .WithMany(a => a.People)
        //.HasForeignKey(p => p.AdressId);
        .UsingEntity(join => join.HasData([
            new{PeopleId = "PERS01", AdressesId = adressId1},
            new{PeopleId = "PERS01", AdressesId = adressId2},
            new{PeopleId = "PERS02", AdressesId = adressId2},
            new{PeopleId = "PERS03", AdressesId = adressId1}
        ]));
        

    base.OnModelCreating(modelBuilder);
    }
    
}