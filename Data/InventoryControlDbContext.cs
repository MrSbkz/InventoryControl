using InventoryControl.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data
{
    public class InventoryControlDbContext:IdentityDbContext<User>
    {
        public InventoryControlDbContext(DbContextOptions<InventoryControlDbContext> options) : base(options)
        { }
    }
}
