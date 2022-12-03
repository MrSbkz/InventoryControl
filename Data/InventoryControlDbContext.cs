using InventoryControl.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data
{
    public class InventoryControlDbContext:IdentityDbContext<User>
    {
        public DbSet<DeviceHistory> DeviceHistory { get; set; }

        public DbSet<Device> Device { get; set; }

        public DbSet<Inventory> Inventory { get; set; }

        public DbSet<Ticket> Ticket { get; set; }

        public DbSet<TicketStatus> TicketStatus { get; set; }

        public InventoryControlDbContext(DbContextOptions<InventoryControlDbContext> options) : base(options)
        { }
    }
}
