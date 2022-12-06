namespace InventoryControl.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int DeviceId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedById { get; set; }=string.Empty;

        public int StatusId { get; set; }

        public string? AssignedToId { get; set; }

        public DateTime? CompletionDate { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Device Device { get; set; }

        public User CreatedBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public User? AssignedTo { get; set; }
    }
}
