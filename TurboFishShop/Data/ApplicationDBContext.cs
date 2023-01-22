using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TurboFishShop.Models;

namespace TurboFishShop.Data
{
    public class ApplicationDBContext : IdentityDbContext  // изменили наследование
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) :
            base(options) 
        {

        }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<MyModel> MyModels { get; set; }
		public DbSet<Product> Product { get; set; }

		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
	}
}
