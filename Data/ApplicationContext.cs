using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FurnitureStore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {   public DbSet<Furniture> Furniture { get; set; }
        public DbSet<FurnitureType> FurnitureType { get; set; }
        public DbSet<FurnitureKind> FurnitureKind { get; set; }
        public DbSet<FurnitureColor> FurnitureColor { get; set; }
        public DbSet<ShopCartItem> ShopCartItem { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<HomePage> HomePage { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { Database.EnsureCreated(); }


    }
}
