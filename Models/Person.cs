
namespace PersonApi.Models;

    public class Person(string name, int age, bool isMarried)
{
    public string Id { get; } = Guid.NewGuid().ToString()[..6];
    public string Name { get; set; } = name;
    public int Age { get; set; } = age;
    public bool IsMarried { get; set; } = isMarried;
    //public Address Address { get; set; } = address;
}