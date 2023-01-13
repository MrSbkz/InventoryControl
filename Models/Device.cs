namespace InventoryControl.Models;

public class Device
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public Employee AssignedTo { get; set; }
    
    public DateTime RegisterDate { get; set; }
    
    public DateTime DecommissionDate { get; set; }
}