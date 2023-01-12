namespace InventoryControl.Models;

public class UserDto
{
    public string UserName { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public IList<string>? Roles { get; set; }
}