namespace StoreAPI.Repositories
{
    using StoreAPI.Models;

    /// <summary>
    /// Interface for managing Wishlist data operations
    /// </summary>
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
