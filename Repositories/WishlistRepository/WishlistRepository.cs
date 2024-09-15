namespace StoreAPI.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using StoreAPI.Data;
    using StoreAPI.Models;

    /// <summary>
    /// Repository implementation for managing wishlist
    /// </summary>
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistRepository"/> class
        /// </summary>
        /// <param name="context">The application database context</param>
        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all wishlists including their associated products
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IEnumerable<Wishlist>> GetAllWishListAsync()
        {
            return await _context.Wishlists
                .Include(w => w.WishlistProducts)
                .ThenInclude(wp => wp.Product)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a wishlist by its name, including its associated products
        /// </summary>
        /// <param name="name">The name of the wishlist</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<Wishlist> GetWishlistByNameAsync(string name)
        {
            return await _context.Wishlists
                .Include(w => w.WishlistProducts)
                .ThenInclude(wp => wp.Product)
                .FirstOrDefaultAsync(w => w.Name == name);
        }

        /// <summary>
        /// Creates a new wishlist and saves it to the database
        /// </summary>
        /// <param name="wishlist">The wishlist to create</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task CreateWishlistAsync(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds a product to a wishlist
        /// </summary>
        /// <param name="wishlist">The wishlist to which the product will be added</param>
        /// <param name="product">The product to add</param>
        /// <returns>A task that represents the asynchronous operation</returns>
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

        /// <summary>
        /// Removes a product from a wishlist
        /// </summary>
        /// <param name="wishlist">The wishlist from which the product will be removed</param>
        /// <param name="productId">The ID of the product to remove</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task RemoveProductFromWishlistAsync(Wishlist wishlist, int productId)
        {
            var wishlistProduct = wishlist.WishlistProducts.FirstOrDefault(wp => wp.ProductId == productId);
            if (wishlistProduct != null)
            {
                wishlist.WishlistProducts.Remove(wishlistProduct);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Checks if a product is in a wishlist
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist</param>
        /// <param name="productId">The ID of the product</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<bool> IsProductInWishlistAsync(int wishlistId, int productId)
        {
            return await _context.WishlistProducts
                .AnyAsync(wp => wp.WishlistId == wishlistId && wp.ProductId == productId);
        }

        /// <summary>
        /// Deletes a wishlist by its name
        /// </summary>
        /// <param name="name">The name of the wishlist to delete</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        /// <exception cref="Exception">Thrown when the wishlist with the specified name is not found</exception>
        public async Task DeleteWishlistByNameAsync(string name)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistProducts)
                .FirstOrDefaultAsync(w => w.Name == name);

            if (wishlist == null)
            {
                throw new Exception($"Wishlist {name} not found");
            }

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
        }
    }
}
