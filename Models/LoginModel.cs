using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserNAme is required")]
        public string UserName { get; set; }=string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } =string.Empty;
    }
}
