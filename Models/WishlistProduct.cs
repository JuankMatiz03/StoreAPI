namespace StoreAPI.Models
{
    /// <summary>
    /// Intermediate entity for Wishlist and Product
    /// </summary>
    public class WishlistProduct
    {
        public int WishlistId { get; set; }
        public required Wishlist Wishlist { get; set; }

        public int ProductId { get; set; }
        public required Product Product { get; set; }
    }
}
