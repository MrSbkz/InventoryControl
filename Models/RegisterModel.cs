using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }=string.Empty;

        public string FirstName { get; set; }=string.Empty;

        public string LastName { get; set; }=string.Empty;

        public List<string>? Roles { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

    }
}
