namespace StoreAPI.Models
{
    /// <summary>
    /// Represents a Category.
    /// </summary>
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Product> Products { get; set; } = new();
    }

}
