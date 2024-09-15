namespace StoreAPI.Repositories
{
    using StoreAPI.Models;

    /// <summary>
    /// Interface for managing Product data operations
    /// </summary>
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task CreateProductsAsync(Product entity);
        Task UpdateProductsAsync(Product entity);
        Task DeleteProductsAsync(int id);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Product> GetProductByNameAsync(string name);
    }
}
