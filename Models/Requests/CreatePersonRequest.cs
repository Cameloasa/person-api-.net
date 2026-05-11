using System.ComponentModel.DataAnnotations;

namespace PersonApi.Models.Requests;
public struct CreatePersonRequest {
    [Required]
    public string Name { get; set; }
    [Required]
    public int Age { get; set; }
    [Required]
    public bool IsMarried { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]  
    public string Zip { get; set; }
    [Required]
    public string City { get; set; }


}

//{
//    "Name": "Clearence Donegan",
//    "Age": 35,
//    "IsMarried": true,
//    "Street": "Main Street 1",
//    "Zip": "12345",
//    "City": "Anytown"
//}