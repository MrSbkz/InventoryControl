namespace InventoryControl.Data.Entities
{
    public class Device
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime RegisterDate { get; set; }

        public int UserId { get; set; }

        public DateTime DecommissionDateP { get; set; }
    }
}
