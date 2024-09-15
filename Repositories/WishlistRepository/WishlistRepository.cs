using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Models;

namespace StoreAPI.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wishlist>> GetAllWishListAsync()
        {
            return await _context.Wishlists
                .Include(w => w.WishlistProducts)
                .ThenInclude(wp => wp.Product)
                .ToListAsync();
        }

        public async Task<Wishlist> GetWishlistByNameAsync(string name)
        {
            return await _context.Wishlists
                .Include(w => w.WishlistProducts)
                .ThenInclude(wp => wp.Product)
                .FirstOrDefaultAsync(w => w.Name == name);
        }

        public async Task CreateWishlistAsync(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
        }

        public async Task AddProductToWishlistAsync(Wishlist wishlist, Product product)
        {
            wishlist.WishlistProducts.Add(new WishlistProduct
            {
                WishlistId = wishlist.Id,
                Wishlist = wishlist, 
                ProductId = product.Id,
                Product = product 
            });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveProductFromWishlistAsync(Wishlist wishlist, int productId)
        {
            var wishlistProduct = wishlist.WishlistProducts.FirstOrDefault(wp => wp.ProductId == productId);
            if (wishlistProduct != null)
            {
                wishlist.WishlistProducts.Remove(wishlistProduct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsProductInWishlistAsync(int wishlistId, int productId)
        {
            return await _context.WishlistProducts
                .AnyAsync(wp => wp.WishlistId == wishlistId && wp.ProductId == productId);
        }


        public async Task DeleteWishlistByNameAsync(string name)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistProducts)
                .FirstOrDefaultAsync(w => w.Name == name);

            if (wishlist == null)
            {
                throw new Exception($"Wishlist {name} not found.");
            }

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
        }
    }
}
