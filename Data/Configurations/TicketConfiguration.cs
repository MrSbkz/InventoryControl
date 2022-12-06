using InventoryControl.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data.Configurations
{
    public class TicketConfiguration
    {
        public static void Create(ModelBuilder builder)
        {
            builder.Entity<Ticket>()
                .HasOne(x => x.Device)
                .WithMany()
                .HasForeignKey(x => x.DeviceId);

            builder.Entity<Ticket>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById);

            builder.Entity<Ticket>()
                .HasOne(x => x.AssignedTo)
                .WithMany()
                .HasForeignKey(x => x.AssignedToId);
        }
    }
}
