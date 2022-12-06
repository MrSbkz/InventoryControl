namespace InventoryControl.Data.Entities
{
    public class Inventory
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public string CretedById { get; set; } 

        public DateTime InventoryDate { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Device Device { get; set; }

        public User CretedBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
