using StoreAPI.Models;

namespace StoreAPI.Repositories
{
    public interface IWishlistRepository
    {
        Task<Wishlist> GetWishlistByNameAsync(string name);
        Task CreateWishlistAsync(Wishlist wishlist);
        Task AddProductToWishlistAsync(Wishlist wishlist, Product product);
        Task RemoveProductFromWishlistAsync(Wishlist wishlist, int productId);
        Task DeleteWishlistByNameAsync(string name);
        Task<IEnumerable<Wishlist>> GetAllWishListAsync();
        Task<bool> IsProductInWishlistAsync(int wishlistId, int productId);
    }
}
