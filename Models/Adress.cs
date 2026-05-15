namespace PersonApi.Models;

    public class Adress(string street, string zip, string city)
{
    public string Id {get; set;} = Guid.NewGuid().ToString()[..6];
    public string Street { get; set; } = street;
    public string Zip { get; set; } = zip;
    public string City { get; set; } = city;

    //relation to tabel People
    public ICollection<Person> People { get; set; } = new List<Person>();
}