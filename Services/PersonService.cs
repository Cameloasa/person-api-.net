//src/Services/PersonService.cs
namespace PersonApi.Services;

using PersonApi.Models;
using PersonApi.Models.Requests;
using PersonApi.Repositories;

public interface IPersonService
{
    IEnumerable<Person> GetPersons();
    Person? GetPersonById(string id);
    Task<Person> CreatePerson(CreatePersonRequest request);
    Task<Person?> UpdatePerson(string id,CreatePersonRequest request);
    Task<bool> DeletePerson(string id);
}
public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository repo)
    {
        _personRepository = repo;
    }
   
    //create
    public async Task<Person> CreatePerson(CreatePersonRequest request)
    {
            Person newPerson = new(
            request.Name,
            request.Age,
            request.IsMarried
        );

        // 1. If existing adress -> use it
        if (!string.IsNullOrEmpty(request.AdressId))
        {
            newPerson.AdressId = request.AdressId;
        }
        else
        {
            // 2. If not AdressId → create a new one
            newPerson.Adress = new Adress(
                request.Street!,
                request.Zip!,
                request.City!
            );
        }

        return await _personRepository.CreatePerson(newPerson);
    }

    //delete
    public async Task<bool> DeletePerson(string id)
    {
        try
        {
            var exists = _personRepository.GetPersonById(id);
            if (exists == null)
                return false;

            return await _personRepository.DeletePerson(id);
        }
        catch
        {
            throw;
        }
    }

    // get all persons
    public IEnumerable<Person> GetPersons()
    {
        return _personRepository.GetAllPersons();
    }

    // get person by id
    public Person? GetPersonById(string id)
    {
         return _personRepository.GetPersonById(id);       
    }

    //update
    public async Task<Person?> UpdatePerson(string id, CreatePersonRequest request)
    {
        try
        {
            var existingPerson = _personRepository.GetPersonById(id);
            if (existingPerson == null)
                return null;

            var personToUpdate = new Person(
                request.Name,
                request.Age,
                request.IsMarried
            );

            // 1. If user sent AdressId → change to a new adress
            if (!string.IsNullOrEmpty(request.AdressId))
            {
                personToUpdate.AdressId = request.AdressId;
            }
            // 2. If user send Street/Zip/City → create a new adress
            else if (request.Street != null && request.Zip != null && request.City != null)
            {
                personToUpdate.Adress = new Adress(
                    request.Street!,
                    request.Zip!,
                    request.City!
                );
            }
            // 3. If user doesn't send adrees id → change only name
            else
            {
                personToUpdate.AdressId = existingPerson.AdressId;
            }

            var updated = await _personRepository.UpdatePerson(id, personToUpdate);
            return updated;
        }
        catch
        {
            throw;
        }
    }
}