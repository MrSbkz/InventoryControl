namespace InventoryControl.Data.Entities
{
    public class DeviceHistory
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public DateTime RepairDate { get; set; }

        public bool IsRepaired { get; set; }
    }
}
