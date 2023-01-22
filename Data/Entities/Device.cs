namespace InventoryControl.Data.Entities
{
    public class Device
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime RegisterDate { get; set; }

        public string? UserId { get; set; }

        public DateTime? DecommissionDate { get; set; }

        public User? User { get; set; }
    }
}