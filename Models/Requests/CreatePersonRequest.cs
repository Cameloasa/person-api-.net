namespace PersonApi.Models.Requests;
public struct CreatePersonRequest {
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsMarried { get; set; }
    public string Street { get; set; }
    public string Zip { get; set; }
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