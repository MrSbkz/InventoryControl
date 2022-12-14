namespace InventoryControl.Models
{
    public class AuthResponse
    {
        public string Status { get; set; }=string.Empty;

        public IList<string>? Reasons { get; set; } 
    }
}
