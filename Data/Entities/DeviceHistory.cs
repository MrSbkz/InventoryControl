namespace InventoryControl.Data.Entities
{
    public class DeviceHistory
    {
        public int Id { get; set; }

        public string Action { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; }

        public int DeviceId { get; set; }

        public Device? Device { get; set; }
        
    }
}