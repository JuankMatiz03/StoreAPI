namespace StoreAPI.Models
{ 
    /// <summary>
    /// Represents a Product.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public ICollection<WishlistProduct> WishlistProducts { get; set; } = new List<WishlistProduct>();
    }
}
