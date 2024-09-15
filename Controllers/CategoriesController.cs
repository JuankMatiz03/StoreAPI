using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreAPI.Models;
using StoreAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<CategoryController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class
        /// </summary>
        /// <param name="repository">The category repository</param>
        /// <param name="logger">The logger</param>
        public CategoryController(ICategoryRepository repository, ILogger<CategoryController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all categories
        /// </summary>
        /// <returns>A task that represents the asynchronous operation with a collection of categories</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try 
            {
                var categories = await _repository.GetAllCategoriesAsync();

                if (categories == null || !categories.Any())
                {
                    _logger.LogWarning("****NO CATEGORIES FOUND.****");
                    return StatusCode(400, new 
                    { 
                        Message = "No Categories found",
                        Data = new List<Category>()  
                    });
                }

                _logger.LogInformation("****SUCCESSFULLY RETRIEVED ALL CATEGORIES.****");
                return StatusCode(200, new 
                {
                    Message = "Success GET LIST",
                    Data = categories 
                });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE GETTING ALL CATEGORIES.****");
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while getting all categories",
                    Error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Retrieves a category by its ID
        /// </summary>
        /// <param name="id">The ID of the category</param>
        /// <returns>A task that represents the asynchronous operation with the category if found</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            try
            {
                var category = await _repository.GetCategoryByIdAsync(id);
                if (category == null) 
                {
                    _logger.LogWarning("****CATEGORY WITH ID {CategoryId} NOT FOUND****", id);
                    return StatusCode(400, new 
                    {
                        Message = $"Category with ID {id} not found",
                        Data = new List<Category>() 
                    });
                }

                _logger.LogInformation("****SUCCESSFULLY RETRIEVED CATEGORY WITH ID {CategoryId}****", id);
                return StatusCode(200, new 
                {
                    Message = "Success",
                    Data = category 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE GETTING CATEGORY WITH ID {CategoryId}****", id);
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while getting the category",
                    Error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <param name="category">The category to create</param>
        /// <returns>A task that represents the asynchronous operation with the created category</returns>
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            try 
            {
                if (category == null)
                {
                    _logger.LogWarning("****CATEGORY OBJECT IS NULL****");
                    return StatusCode(400, new 
                    { 
                        Message = "Category object is null",
                    });
                }

                var existingCategory = await _repository.GetCategoryByNameAsync(category.Name);
                if (existingCategory != null)
                {
                    _logger.LogWarning("****CATEGORY WITH NAME {CategoryName} ALREADY EXISTS****", category.Name);
                    return StatusCode(400, new 
                    { 
                        Message = $"Category with name '{category.Name}' already exists.",
                    });
                }

                await _repository.CreateCategoryAsync(category);
                _logger.LogInformation("****SUCCESSFULLY CREATED CATEGORY WITH NAME {CategoryName}****", category.Name);
                return StatusCode(201, new 
                {
                    Message = "Category created successfully.",
                    Data = category
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE CREATING THE CATEGORY****");
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while creating the category",
                    Error = ex.Message 
                });
            }
        }

        /// <summary>
        /// Updates an existing category
        /// </summary>
        /// <param name="id">The ID of the category to update</param>
        /// <param name="category">The updated category information</param>
        /// <returns>A task that represents the asynchronous operation indicating the outcome of the operation</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            try
            {
                if (id != category.Id) 
                {
                    _logger.LogWarning("****CATEGORY ID MISMATCH. PROVIDED ID: {ProvidedId}, CATEGORY ID: {CategoryId}****", id, category.Id);
                    return StatusCode(400, new 
                    {
                        Message = $"Category ID mismatch. Provided ID: {id}, Category ID: {category.Id}",
                        Data = new List<Category>() 
                    });
                }

                var existingCategory = await _repository.GetCategoryByIdAsync(id);
                if (existingCategory == null) 
                {
                    _logger.LogWarning("****CATEGORY WITH ID {CategoryId} NOT FOUND****", id);
                    return StatusCode(400, new 
                    {
                        Message = $"Category with ID {id} not found",
                        Data = new List<Category>() 
                    });
                }

                await _repository.UpdateCategoryAsync(category);
                _logger.LogInformation("****SUCCESSFULLY UPDATED CATEGORY WITH ID {CategoryId}****", id);
                return StatusCode(200, new 
                {
                    Message = $"Category ID {id} updated successfully",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE UPDATING CATEGORY WITH ID {CategoryId}****", id);
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while updating the category",
                    Error = ex.Message 
                });
            } 
        }

        /// <summary>
        /// Deletes a category by its ID
        /// </summary>
        /// <param name="id">The ID of the category to delete</param>
        /// <returns>A task that represents the asynchronous operation indicating the outcome of the operation</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _repository.GetCategoryByIdAsync(id);
                if (category == null) 
                {
                    _logger.LogWarning("****CATEGORY WITH ID {CategoryId} NOT FOUND****", id);
                    return StatusCode(400, new 
                    {
                        Message = $"Category with ID {id} not found",
                        Data = new List<Category>() 
                    });
                }

                await _repository.DeleteCategoryAsync(id);
                _logger.LogInformation("****SUCCESSFULLY DELETED CATEGORY WITH ID {CategoryId}****", id);
                return StatusCode(200, new 
                {
                    Message = $"Category ID {id} deleted successfully.",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE DELETING CATEGORY WITH ID {CategoryId}****", id);
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while deleting the category",
                    Error = ex.Message 
                });
            }
        }
    }
}
