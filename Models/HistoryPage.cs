using InventoryControl.Data.Entities;

namespace InventoryControl.Models;

public class HistoryPage
{
    public string Name { get; set; } = String.Empty;
    
    public Employee AssignedTo { get; set; } 

    public DateTime RegisterData { get; set; }

    public List<Inventory>? Inventories { get; set; }

    public DateTime? DecommissionDate { get; set; }
    
}