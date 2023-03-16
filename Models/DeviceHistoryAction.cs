namespace InventoryControl.Models;

public static class DeviceHistoryAction
{
    public static string AssignedTo => "Assigned to {0} ({1})";
    public static string InventoryBy => "Inventory by {0} ({1})";
    public static string UpdateDeviceName => "Update device name from {0} to {1}";
    public static string UpdateAssigning => "Updated user assigning from {0} to {1} ";
    public static string DecommissionedBy => "Decommissioned by {0} ({1})";
}