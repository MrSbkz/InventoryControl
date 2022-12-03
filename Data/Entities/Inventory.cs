namespace InventoryControl.Data.Entities
{
    public class Inventory
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public DateTime InventoryDate { get; set; }
    }
}
