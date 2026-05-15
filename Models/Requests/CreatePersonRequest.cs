
using PersonApi.Models.DTOs;

namespace PersonApi.Models.Requests;
public class CreatePersonRequest
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool IsMarried { get; set; }
    public List<AdressDTO> Adresses { get; set; } = new();
}
//{
//    "Name": "Clearence Donegan",
//    "Age": 35,
//    "IsMarried": true,
//    "Street": "Main Street 1",
//    "Zip": "12345",
//    "City": "Anytown"
//}