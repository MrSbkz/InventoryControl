namespace InventoryControl.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Topic { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int DeviceId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }

        public int StatusId { get; set; }

        public int AssignedTo { get; set; }

        public DateTime CompleteDate { get; set; }
    }
}
