using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Login is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}
