using InventoryControl.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data.Configurations
{
    public class InventoryConfiguration
    {
        public static void Create(ModelBuilder builder)
        {
            builder.Entity<Inventory>()
                .HasOne(x => x.Device)
                .WithMany()
                .HasForeignKey(x => x.DeviceId);

            builder.Entity<Inventory>()
                .HasOne(x => x.CretedBy)
                .WithMany()
                .HasForeignKey(x => x.CretedById);
        }
    }
}
