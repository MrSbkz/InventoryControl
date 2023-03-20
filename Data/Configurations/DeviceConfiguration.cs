using InventoryControl.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data.Configurations
{
    public static class DeviceConfiguration
    {
        public static void Create(ModelBuilder builder)
        {
            builder.Entity<Device>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
