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
        Adress newAdress = new Adress
        (
            request.Street,
            request.Zip,
            request.City
        );
        Person newPerson = new(request.Name,
                                request.Age,
                                request.IsMarried
                                )
        {
            Adress = newAdress
        };

        Person createdPerson = await _personRepository.CreatePerson(newPerson);
        
        return createdPerson;
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
            // 1. Verify if person exists
            var existingPerson = _personRepository.GetPersonById(id);
            if (existingPerson == null)
                return null;

            // 2. Map model request
            var personToUpdate = new Person(
                request.Name,
                request.Age,
                request.IsMarried
            );

            // 3. Call repository 
            var updated = await _personRepository.UpdatePerson(id, personToUpdate);

            //return
            return updated;
        }
        catch
        {
            throw;
        }
    }

}