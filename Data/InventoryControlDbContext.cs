using InventoryControl.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Data
{
    public class InbentoryControlDbContext:IdentityDbContext<User>
    {
        public InbentoryControlDbContext(DbContextOptions<InbentoryControlDbContext> options) : base(options)
        { }
    }
}
