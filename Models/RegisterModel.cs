﻿using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Login is required")]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> Role { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}
