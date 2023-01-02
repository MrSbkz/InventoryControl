namespace InventoryControl.Models;

public class UserModel
{
    public int Id { get; set; }
    
    public string UserName { get; set; } = string.Empty;
    
    public string FistName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}