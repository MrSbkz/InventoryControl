namespace InventoryControl.Models;

public class QrCodeModel
{
    public Stream? Path { get; set; } 
    
    public string Type { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
}