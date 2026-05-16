

namespace PersonApi.Models;

    public class Person
{
    public string Id { get; set; } = Guid.NewGuid().ToString()[..6];
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool IsMarried { get; set; }

    public List<Adress> Adresses { get; set; } = new();

    public Person(string name, int age, bool isMarried)
    {
        Name = name;
        Age = age;
        IsMarried = isMarried;
    }

    public Person()
    {
        
    } // EF Core needs this
}