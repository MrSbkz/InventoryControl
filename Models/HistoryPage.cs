using InventoryControl.Data.Entities;

namespace InventoryControl.Models;

public class HistoryPage
{
    public string Action { get; set; } = string.Empty;
    
    public DateTime? CreatedDate { get; set; }

}