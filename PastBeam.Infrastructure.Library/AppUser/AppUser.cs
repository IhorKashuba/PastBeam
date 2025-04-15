using Microsoft.AspNetCore.Identity;

namespace PastBeam.Core.Library.Entities
{
    public class AppUser : IdentityUser
    {
        public bool IsSuspended { get; set; }

        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
