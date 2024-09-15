namespace StoreAPI.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<WishlistProduct> WishlistProducts { get; set; } = new List<WishlistProduct>();
    }
}
