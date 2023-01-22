namespace InventoryControl.Models;

public class UpdateDeviceModel
{
    public int Id { get; set; }

    public string? Name { get; set; } = string.Empty;

    public string? AssignedTo { get; set; } = string.Empty;
}