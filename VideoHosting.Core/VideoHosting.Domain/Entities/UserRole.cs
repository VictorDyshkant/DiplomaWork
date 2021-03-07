using Microsoft.AspNetCore.Identity;


namespace VideoHosting.Domain.Entities
{
    public class UserRole : IdentityRole
    {
       public string Description { get; set; }
    }
}
