using InventoryControl.Data.Configurations;
using InventoryControl.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data
{
    public  class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<DeviceHistory> DeviceHistories { get; set; }

        public DbSet<Device> Devices { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketStatus> TicketStatuses { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            DeviceConfiguration.Create(builder);
            DeviceHistoryConfiguration.Create(builder);
            InventoryConfiguration.Create(builder);
            TicketConfiguration.Create(builder);
        }
    }
}
