namespace StoreAPI.Repositories
{
    using StoreAPI.Models;

    /// <summary>
    /// Interface for managing Category data operations
    /// </summary>
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(Category entity);
        Task UpdateCategoryAsync(Category entity);
        Task DeleteCategoryAsync(int id);
        Task<Category> GetCategoryByNameAsync(string name);
        Task<bool> CategoryExistsAsync(int categoryId);
    }
}
