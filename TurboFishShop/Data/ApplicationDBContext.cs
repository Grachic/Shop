using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TurboFishShop.Models;

namespace TurboFishShop.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) :
            base(options) 
        {

        }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<MyModel> MyModels { get; set; } 

        public DbSet<Product> Product { get; set; }
    }
}
