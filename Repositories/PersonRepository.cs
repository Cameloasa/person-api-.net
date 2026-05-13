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
    
    //create
    public async Task<Person> CreatePerson(Person personToSave)
    {
        try{
            var result = await _context.People.AddAsync(personToSave);
            int changes = await _context.SaveChangesAsync();
            if(changes > 0)
            {
                return result.Entity;
            }
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

        return _context.People.Include(p => p.Adress).ToList();
    }

    public Person? GetPersonById(string id)
    {
        return _context.People.Include(p => p.Adress).ThenInclude(a => a.People).First(p => p.Id == id);
    }

    //update
   public async Task<Person?> UpdatePerson(string id, Person personToUpdate)
    {
        try
        {
            // 1. Find person in DB
            var existingPerson = await _context.People.FindAsync(id);

            // 2. If not existing return null
            if (existingPerson == null)
                return null;

            // 3. Uptade fields
            existingPerson.Name = personToUpdate.Name;
            existingPerson.Age = personToUpdate.Age;
            existingPerson.IsMarried = personToUpdate.IsMarried;

            // 4. Save in Db
            await _context.SaveChangesAsync();

            return existingPerson;
        }
        catch
        {
            throw;
        }
    }
        
}