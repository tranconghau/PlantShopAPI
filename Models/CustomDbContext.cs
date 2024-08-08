using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlantShopAPI.Models;

namespace PlantShopAPI.Models
{
    public class CustomDbContext : IdentityDbContext<CustomUser, IdentityRole, string>
    /*public class CustomDbContext : IdentityDbContext<IdentityUser>*/
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options) 
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<PlantShopAPI.Models.Product> Product { get; set; } = default!;
    }
}
