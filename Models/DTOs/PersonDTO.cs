namespace PersonApi.Models.DTOs;
public class PersonDTO
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; } 
   
    //reference to adresses
     public ICollection<AdressDTO> Adresses { get; set; } = [];
}