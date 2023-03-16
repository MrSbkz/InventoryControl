using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Enums;

public enum DeviceHistoryAction
{
    [Display(Name = "Assigned to {0} ({1})")]
    AssignedTo,

    [Display(Name = "Inventory by {0} ({1})")]
    InventoryBy,

    [Display(Name = "Update device name from {0} to {1}")]
    UpdateDeviceName,

    [Display(Name = "Updated user assigning from {0} to {1}")]
    UpdateAssigning,

    [Display(Name = "Decommissioned by {0} ({1})")]
    DecommissionedBy,

    [Display(Name = "Updated an unassigned user to {0} ")]
    Unassignedto,

    [Display(Name = "Updated user assigned from {0} to unassigned user")]
    DeviceToUnassigned
}