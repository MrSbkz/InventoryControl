namespace InventoryControl.Models;

public class UpdateUserModel
{
    public string UserName { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
#pragma warning disable CS8618
    public List<string> Roles { get; set; } 
#pragma warning restore CS8618
}