using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Bcpg;

namespace BackendProject.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
        public Basket Basket { get; set; }

    }

}
