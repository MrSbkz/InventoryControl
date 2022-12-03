namespace InventoryControl.Data.Entities
{
    public class Divece
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime RegisterDate { get; set; } = DateTime.MinValue;

        public int UserId { get; set; }

        public DateTime DecommissionDateP { get; set; }
    }
}
