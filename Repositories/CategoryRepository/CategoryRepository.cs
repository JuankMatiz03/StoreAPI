namespace StoreAPI.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using StoreAPI.Data;
    using StoreAPI.Models;

    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category ?? throw new Exception($"Category with ID {id} not found");
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task CreateCategoryAsync(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category entity)
        {
            var existingCategory = await _context.Categories.FindAsync(entity.Id);

            _context.Entry(existingCategory).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }
    }
}
