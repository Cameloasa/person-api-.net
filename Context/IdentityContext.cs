
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonApi.Models;

namespace PersonApi.Context;
public class IdentityContext : IdentityDbContext<User>
{
    public IdentityContext(DbContextOptions options) : base(options)
    {
        
    }
}