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
    // many-to-many helper
    Task<List<Adress>> GetOrCreateAdresses(List<Adress> adresses);
    
}
public class PersonRepository (ApplicationDbContext context) : IPersonRepository
{

    private readonly ApplicationDbContext _context = context;
    
    //create person
    public async Task<Person> CreatePerson(Person personToSave)
    {
        try
        {
            // 1. final list for adress
            var finalAdresses = await GetOrCreateAdresses(personToSave.Adresses.ToList());

            // 2. add final list
            personToSave.Adresses.Clear();
            foreach (var a in finalAdresses)
                personToSave.Adresses.Add(a);

            // 3. Save person
            await _context.People.AddAsync(personToSave);
            await _context.SaveChangesAsync();

            return personToSave;
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
            // 1. Find person in db
            var existingPerson = await _context.People
                .Include(p => p.Adresses)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPerson == null)
                return null;

            // 2. Update person
            existingPerson.Name = personToUpdate.Name;
            existingPerson.Age = personToUpdate.Age;
            existingPerson.IsMarried = personToUpdate.IsMarried;

            // 3. Update adrese (many-to-many)
            var finalAdresses = await GetOrCreateAdresses(personToUpdate.Adresses.ToList());

            // clear old list
            existingPerson.Adresses.Clear();

            // add new list
            foreach (var a in finalAdresses)
                existingPerson.Adresses.Add(a);

            // 4. Save
            await _context.SaveChangesAsync();

            return existingPerson;
        }
        catch
        {
            throw;
        }
    }
    //helper for adress
    public async Task<List<Adress>> GetOrCreateAdresses(List<Adress> adresses)
    {
        var finalList = new List<Adress>();

        foreach (var adress in adresses)
        {
            // Search if adress is in the database
            var existing = await _context.Adresses
                .FirstOrDefaultAsync(a =>
                    a.Street == adress.Street &&
                    a.Zip == adress.Zip &&
                    a.City == adress.City);

            if (existing != null)
            {
                // use existing adress
                finalList.Add(existing);
            }
            else
            {
                // Find the nrew adress
                await _context.Adresses.AddAsync(adress);
                await _context.SaveChangesAsync();
                finalList.Add(adress);
            }
        }

        return finalList;
    }
}