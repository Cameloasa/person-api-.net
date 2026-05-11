//src/Repositories/PersonRepository.cs
using PersonApi.Models;
namespace PersonApi.Repositories;

public interface IPersonRepository
{
    public List<Person> GetAllPersons();
    public Person? GetPersonById(string id);
    public bool CreatePerson(Person person);
    public Person? UpdatePerson(string id, Person personToUpdate);
    public bool DeletePerson(string id);
}
public class PersonRepository : IPersonRepository
{

    private List<Person> persons;
    public PersonRepository()
     {
        persons =
        [
            new Person("John Doe", 30, false, new Address("Main Street 1", "12345", "Anytown")),
            new Person("Jane Smith", 25, true, new Address("Oak Avenue 2", "67890", "Somewhere")),
            new Person("Alice Johnson", 40, true, new Address("Pine Road 3", "54321", "Everytown"))
        ];
    }


    public bool CreatePerson(Person person)
    {
        try{
        persons.Add(person);
        return true;
        }
        catch
        {
            return false;
        }
    }

    public bool DeletePerson(string id)
    {
        var personToDelete = GetPersonById(id);
        if (personToDelete == null) 
            return false;

        persons.Remove(personToDelete);
        return true;
    }

    public List<Person> GetAllPersons()
    {
        return persons;
    }

    public Person? GetPersonById(string id)
    {
        return persons.FirstOrDefault(p => p.Id == id);
    }

    public Person? UpdatePerson(string id, Person personToUpdate)
    {
        var existingPerson = GetPersonById(id);
        if (existingPerson == null) 
            return null;

        existingPerson.Name = personToUpdate.Name;
        existingPerson.Age = personToUpdate.Age;
        existingPerson.IsMarried = personToUpdate.IsMarried;
        existingPerson.Address = personToUpdate.Address;
        return existingPerson;
    }
}