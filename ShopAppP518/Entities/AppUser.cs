using Microsoft.AspNetCore.Identity;

namespace ShopAppP518.Entities
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
