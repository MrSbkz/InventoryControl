using InventoryControl.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data.Configurations
{
    public static class DeviceHistoryConfiguration
    {
        public static void Create(ModelBuilder builder)
        {
            builder.Entity<DeviceHistory>()
                .HasOne(x => x.Device)
                .WithMany()
                .HasForeignKey(x => x.DeviceId);
        }
        
    }
}
