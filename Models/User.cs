
using Microsoft.AspNetCore.Identity;

namespace PersonApi.Models;
public class User: IdentityUser
{
    public string Color {get; set;} = string.Empty;
}