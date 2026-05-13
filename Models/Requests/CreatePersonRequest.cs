using System.ComponentModel.DataAnnotations;
using PersonApi.Validators;

namespace PersonApi.Models.Requests;
public struct CreatePersonRequest {
    
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; }

    //[Required (ErrorMessage = "Age is required.")]
    //[Range(0, 120, ErrorMessage = "Age must be between 0 and 120.")]
    [AgeValidation(18)]
    public int Age { get; set; }

    public bool IsMarried { get; set; }

    //an adress that exists
    public string? AdressId { get; set; }

    //new adress
    public string? Street { get; set; }
    public string? Zip { get; set; }
    public string? City { get; set; }


}

//{
//    "Name": "Clearence Donegan",
//    "Age": 35,
//    "IsMarried": true,
//    "Street": "Main Street 1",
//    "Zip": "12345",
//    "City": "Anytown"
//}