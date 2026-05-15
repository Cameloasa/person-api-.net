
namespace PersonApi.Models;

    public class Person( string name, int age, bool isMarried)
    {
        public string Id { get; set; } = Guid.NewGuid().ToString()[..6];
        public string Name { get; set; } = name;
        public int Age { get; set; } = age;
        public bool IsMarried { get; set; } = isMarried;

        //relation to adress - one to many
        //public string AdressId {get; set;} = null!;
        //public Adress Adress { get; set; } = null!;

        //relation many-to-many
        public ICollection<Adress> Adresses = [];
    }