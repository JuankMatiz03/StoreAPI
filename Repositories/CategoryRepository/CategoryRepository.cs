namespace StoreAPI.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using StoreAPI.Data;
    using StoreAPI.Models;

    /// <summary>
    /// Repository implementation for managing category
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class
        /// </summary>
        /// <param name="context">The application database context</param>
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all categories from the database
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        /// <summary>
        /// Gets a category by its ID
        /// </summary>
        /// <param name="id">The ID of the category</param>
        /// <returns>The task result contains </returns>
        /// <exception cref="Exception">Thrown when the category with the specified ID is not found</exception>
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category ?? throw new Exception($"Category with ID {id} not found");
        }

        /// <summary>
        /// Gets a category by its name
        /// </summary>
        /// <param name="name">The name of the category</param>
        /// <returns>The task result contains the <see cref="Category"/> if found; otherwise, <c>null</c></returns>
        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        /// <summary>
        /// Creates a new category and saves it to the database
        /// </summary>
        /// <param name="entity">The category to create</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task CreateCategoryAsync(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing category in the database
        /// </summary>
        /// <param name="entity">The category with updated values</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task UpdateCategoryAsync(Category entity)
        {
            var existingCategory = await _context.Categories.FindAsync(entity.Id);

            if (existingCategory != null)
            {
                _context.Entry(existingCategory).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a category by its ID
        /// </summary>
        /// <param name="id">The ID of the category to delete</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Checks if a category exists in the database by its ID
        /// </summary>
        /// <param name="categoryId">The ID of the category to check</param>
        /// <returns>The task result contains a boolean indicating whether the category exists</returns>
        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }
    }
}
