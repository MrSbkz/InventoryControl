namespace InventoryControl.Models;

public class UserDTO
{
    
    
    public string UserName { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}