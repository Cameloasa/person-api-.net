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

    [Required(ErrorMessage = "Street is required.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Street must be between 5 and 200 characters.")]
    public string Street { get; set; }

    [Required(ErrorMessage = "Zip code is required.")]
    [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Zip code must be in the format 12345 or 12345-6789.")]
    public string Zip { get; set; }
    
    [Required(ErrorMessage = "City is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters.")]
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