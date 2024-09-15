namespace StoreAPI.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using StoreAPI.Data;
    using StoreAPI.Models;

    /// <summary>
    /// Repository implementation for managing products
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class
        /// </summary>
        /// <param name="context">The application database context</param>
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all products from the database
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Gets a product by its ID
        /// </summary>
        /// <param name="id">The ID of the product</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        /// <exception cref="Exception">Thrown when the product with the specified ID is not found</exception>
        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product ?? throw new Exception($"Product with ID {id} not found");
        }

        /// <summary>
        /// Creates a new product and saves it to the database
        /// </summary>
        /// <param name="entity">The product to create</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task CreateProductsAsync(Product entity)
        {
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing product in the database
        /// </summary>
        /// <param name="entity">The product with updated values</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task UpdateProductsAsync(Product entity)
        {
            var existingProduct = await _context.Products.FindAsync(entity.Id);
            if (existingProduct != null)
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a product by its ID
        /// </summary>
        /// <param name="id">The ID of the product to delete</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteProductsAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets a product by its name
        /// </summary>
        /// <param name="name">The name of the product</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<Product> GetProductByNameAsync(string name)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        /// <summary>
        /// Gets a category by its ID
        /// </summary>
        /// <param name="id">The ID of the category</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category; 
        }
    }
}
