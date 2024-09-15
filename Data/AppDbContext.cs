using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;

namespace StoreAPI.Data
{
    /// <summary>
    /// Represents the application's database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistProduct> WishlistProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura la clave compuesta en la entidad intermedia WishlistProduct
            modelBuilder.Entity<WishlistProduct>()
                .HasKey(wp => new { wp.WishlistId, wp.ProductId });

            // Configura la relación de muchos a muchos entre Wishlist y Product
            modelBuilder.Entity<WishlistProduct>()
                .HasOne(wp => wp.Wishlist)
                .WithMany(w => w.WishlistProducts)
                .HasForeignKey(wp => wp.WishlistId);

            modelBuilder.Entity<WishlistProduct>()
                .HasOne(wp => wp.Product)
                .WithMany(p => p.WishlistProducts)
                .HasForeignKey(wp => wp.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
