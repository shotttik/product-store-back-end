using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProduct> UserProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserProduct>()
                .HasKey(up => new { up.ID_product, up.ID_user });

            builder.Entity<UserProduct>()
                .HasOne(up => up.User)
                .WithMany(up => up.Products)
                .HasForeignKey(up => up.ID_user);
            builder.Entity<UserProduct>()
                .HasOne(up => up.Product)
                .WithMany(up => up.Users)
                .HasForeignKey(up => up.ID_product);

            builder.Entity<Coupon>()
                .HasOne<Transaction>(c => c.Transaction)
                .WithOne(tr => tr.Coupon)
                .HasForeignKey<Transaction>(tr => tr.CouponID)
                .OnDelete(DeleteBehavior.SetNull);
        
        }
        

        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
