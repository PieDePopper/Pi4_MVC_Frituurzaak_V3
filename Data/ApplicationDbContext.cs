using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pi4_MVC_Frituur_V3.Models;

namespace Pi4_MVC_Frituurzaak_V3.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Pi4_MVC_Frituur_V3.Models.Item> Item { get; set; } = default!;
        public DbSet<Pi4_MVC_Frituur_V3.Models.Orderregel> Orderregel { get; set; } = default!;
        public DbSet<Pi4_MVC_Frituur_V3.Models.Order> Order { get; set; } = default!;
    }
}