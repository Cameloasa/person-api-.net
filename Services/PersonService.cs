//src/Services/PersonService.cs
namespace PersonApi.Services;

using PersonApi.Models;
using PersonApi.Models.Requests;
using PersonApi.Repositories;

public interface IPersonService
{
    List<Person> GetPersons();
    Person? GetPersonsById(string id);
    Person CreatePerson(CreatePersonRequest request);
    Person? UpdatePerson(string id,CreatePersonRequest request);
    bool DeletePerson(string id);
}
public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository repo)
    {
        _personRepository = repo;
    }
   
    public Person CreatePerson(CreatePersonRequest request)
    {
        Person newPerson = new(request.Name, 
                                request.Age, 
                                request.IsMarried
                                );
        bool succes =  _personRepository.CreatePerson(newPerson);
        if (!succes)
        {
            throw new InvalidOperationException("Failed to create person.");
        }
        return newPerson;
    }

    public bool DeletePerson(string id)
    {
        Person? found = _personRepository.GetPersonById(id); 
        if (found == null)
        {
            return false;
        }
        _personRepository.DeletePerson(id);
        return true;
    }

    public List<Person> GetPersons()
    {
        return _personRepository.GetAllPersons();
    }

    public Person? GetPersonsById(string id)
    {
         Person? found = _personRepository.GetPersonById(id);
            return found;
    }

    public Person? UpdatePerson(string id, CreatePersonRequest request)
    {
        Person? found = _personRepository.GetPersonById(id);
        if (found == null)
        {
            return null;
        }
        found.Name = request.Name;
        found.Age = request.Age;    
        found.IsMarried = request.IsMarried;
        return found;

    }
}