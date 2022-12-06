namespace InventoryControl.Data.Entities
{
    public class DeviceHistory
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public DateTime? RepairedDate { get; set; }

        public bool IsRepaired { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Ticket Ticket { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
