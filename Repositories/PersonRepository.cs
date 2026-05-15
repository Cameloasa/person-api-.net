//src/Repositories/PersonRepository.cs
using Microsoft.EntityFrameworkCore;
using PersonApi.Context;
using PersonApi.Models;
namespace PersonApi.Repositories;

public interface IPersonRepository
{
    public IEnumerable<Person> GetAllPersons();
    public Person? GetPersonById(string id);
    public Task<Person> CreatePerson(Person personToSave);
    public Task<Person?> UpdatePerson(string id, Person personToUpdate);
    public Task<bool> DeletePerson(string id);
    
}
public class PersonRepository (ApplicationDbContext context) : IPersonRepository
{

    private readonly ApplicationDbContext _context = context;
    
    //create person
    public async Task<Person> CreatePerson(Person personToSave)
    {
        try
        {
            // 1. Get adress 
            var adress = await GetOrCreateAdress(personToSave);

            // 2. Link person to adress
            personToSave.AdressId = adress.Id;
            personToSave.Adress = adress;

            // 3. save person to db
            var result = await _context.People.AddAsync(personToSave);
            int changes = await _context.SaveChangesAsync();

            if (changes > 0)
                return result.Entity;

            throw new Exception("Could not add person");
        }
        catch
        {
            throw;
        }
    }

    //delete
    public async Task<bool> DeletePerson(string id)
    {
        try
        {
            //find person to delete
            var personToDelete = await _context.People.FindAsync(id);
            //if not exist return false
            if(personToDelete == null)
            {
                return false;
            }
            //delete person
            _context.People.Remove(personToDelete);
            //save to database
            await _context.SaveChangesAsync();
            return true;

        }
        catch
        {
            throw;
        }
    }

    public IEnumerable<Person> GetAllPersons()
    {

        return _context.People.Include(p => p.Adresses).ToList();
    }

    public Person? GetPersonById(string id)
    {
        return _context.People
        .Include(p => p.Adresses)
        .ThenInclude(a => a.People)
        .FirstOrDefault(p => p.Id == id);
    }

    //update
   public async Task<Person?> UpdatePerson(string id, Person personToUpdate)
    {
        try
        {
            // 1. Find person in DB
            var existingPerson = await _context.People.FindAsync(id);

            if (existingPerson == null)
                return null;

            // 2. Update simple fields
            existingPerson.Name = personToUpdate.Name;
            existingPerson.Age = personToUpdate.Age;
            existingPerson.IsMarried = personToUpdate.IsMarried;

            // 3. Update address (existing or new)
            var adress = await GetOrCreateAdress(personToUpdate);

            existingPerson.AdressId = adress.Id;
            existingPerson.Adress = adress;

            // 4. Save changes
            await _context.SaveChangesAsync();

            return existingPerson;
        }
        catch
        {
            throw;
        }
    }
    //helper for adress
    private async Task<Adress> GetOrCreateAdress(Person person)
    {
        // if we have AdressId → use current adress
        if (!string.IsNullOrEmpty(person.AdressId))
        {
            var existing = await _context.Adresses.FindAsync(person.AdressId)
                ?? throw new Exception("AdressId does not exist.");

            return existing;
        }

        // id we have an obiect Address → create a new adress
        if (person.Adress != null)
        {
            await _context.Adresses.AddAsync(person.Adress);
            await _context.SaveChangesAsync();
            return person.Adress;
        }

        throw new Exception("No address provided.");
    }

}