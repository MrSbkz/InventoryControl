using Microsoft.AspNetCore.Identity;

namespace InventoryControl.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}