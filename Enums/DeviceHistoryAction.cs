using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Enums;

public enum DeviceHistoryAction
{
    [Display(Name = "Assigned to {0}")]
    Assigned,

    [Display(Name = "Inventory by {0} ({1})")]
    Inventory,

    [Display(Name = "Update device name from {0} to {1}")]
    UpdateName,
    
    [Display(Name = "Decommissioned by {0}")]
    Decommissioned,
    
    [Display(Name = "Unassigned device")]
    UnAssigned
}