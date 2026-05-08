namespace PersonApi.Models;

    public class Address(string street, string zip, string city)
{
    public string Street { get; set; } = street;
    public string Zip { get; set; } = zip;
    public string City { get; set; } = city;
}