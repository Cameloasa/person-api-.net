//src/Services/PersonService.cs
namespace PersonApi.Services;

using AutoMapper;
using PersonApi.Models;
using PersonApi.Models.DTOs;
using PersonApi.Models.Requests;
using PersonApi.Repositories;

public interface IPersonService
{
    IEnumerable<PersonDTO> GetPersons();
    Person? GetPersonById(string id);
    Task<Person> CreatePerson(CreatePersonRequest request);
    Task<Person?> UpdatePerson(string id,CreatePersonRequest request);
    Task<bool> DeletePerson(string id);
    
}
public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;

    public PersonService(IPersonRepository repo , IMapper mapper)
    {
        _personRepository = repo;
        _mapper = mapper;
    }
   
    //create
    public async Task<Person> CreatePerson(CreatePersonRequest request)
    {
            //create new person
            Person newPerson = new(
            request.Name,
            request.Age,
            request.IsMarried
        );

            // 2.  DTO -> Model
            var adressModels = request.Adresses
                .Select(a => new Adress(a.Street, a.Zip, a.City))
                .ToList();

            // 3. use  helper many-to-many
            var finalAdresses = await _personRepository.GetOrCreateAdresses(adressModels);

            // 4. add final adress to person
            foreach (var a in finalAdresses)
                newPerson.Adresses.Add(a);

            // 5. save person
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
    public IEnumerable<PersonDTO> GetPersons()
    {
        IEnumerable<Person> persons =  _personRepository.GetAllPersons();

        return _mapper.Map<IEnumerable<PersonDTO>>(persons);
    }

    // get person by id
    public Person? GetPersonById(string id)
    {
         return _personRepository.GetPersonById(id);       
    }

 public async Task<Person?> UpdatePerson(string id, CreatePersonRequest request)
    {
        // 1. person from DB
        var existingPerson = _personRepository.GetPersonById(id);
        if (existingPerson == null)
            return null;

        // 2. update person
        existingPerson.Name = request.Name;
        existingPerson.Age = request.Age;
        existingPerson.IsMarried = request.IsMarried;

        // 3. DTO -> Model
        var adressModels = request.Adresses
            .Select(a => new Adress(a.Street, a.Zip, a.City))
            .ToList();

        // 4. Many-to-many helper
        var finalAdresses = await _personRepository.GetOrCreateAdresses(adressModels);

        // 5. take list of adress
        existingPerson.Adresses.Clear();
        foreach (var a in finalAdresses)
            existingPerson.Adresses.Add(a);

        // 6. send pers to repo
        return await _personRepository.UpdatePerson(id, existingPerson);
    }
}