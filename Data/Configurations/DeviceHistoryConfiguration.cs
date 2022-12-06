using InventoryControl.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data.Configurations
{
    public class DeviceHistoryConfiguration
    {
        public static void Create(ModelBuilder builder)
        {
            builder.Entity<DeviceHistory>()
                .HasOne(x => x.Ticket)
                .WithMany()
                .HasForeignKey(x => x.TicketId);
        }
    }
}
