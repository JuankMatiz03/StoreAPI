using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;
using StoreAPI.Repositories;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository repository, ICategoryRepository categoryRepository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try 
            {
                var products = await _repository.GetAllProductsAsync();

                if (products == null || !products.Any())
                {
                    _logger.LogWarning("****NO PRODUCTS FOUND****");
                    return StatusCode(404, new 
                    { 
                        Message = "No products found",
                        Data = new List<Product>()  
                    });
                }

                _logger.LogInformation("****SUCCESSFULLY RETRIEVED ALL PRODUCTS****");
                return StatusCode(200, new 
                {
                    Message = "Success",
                    Data = products 
                });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE GETTING ALL PRODUCTS****");
                return StatusCode(400, new 
                { 
                    Message = "An error occurred while getting all products",
                    Error = ex.Message 
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try 
            {
                var product = await _repository.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("****PRODUCT WITH ID {ProductId} NOT FOUND****", id);
                    return StatusCode(404, new 
                    {
                        Message = $"Product with ID {id} not found",
                        Data = new List<Product>() 
                    });
                }

                _logger.LogInformation("****SUCCESSFULLY RETRIEVED PRODUCT WITH ID {ProductId}****", id);
                return StatusCode(200, new 
                {
                    Message = "Success",
                    Data = product 
                });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE GETTING PRODUCT WITH ID {ProductId}****", id);
                return StatusCode(400, new 
                { 
                    Message = "An error occurred while getting product",
                    Error = ex.Message 
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            try 
            {
                if (product == null)
                {
                    _logger.LogWarning("****PRODUCT OBJECT IS NULL****");
                    return BadRequest("Product object is null");
                }

                var existingProduct = await _repository.GetProductByNameAsync(product.Name);
                if (existingProduct != null)
                {
                    _logger.LogWarning("****PRODUCT WITH NAME {ProductName} ALREADY EXISTS****", product.Name);
                    return BadRequest(new 
                    {
                        Message = $"Product with name '{product.Name}' already exists."
                    });
                }

                var category = await _repository.GetCategoryByIdAsync(product.CategoryId);
                if (category == null)
                {
                    _logger.LogWarning("****CATEGORY WITH ID {CategoryId} NOT FOUND****", product.CategoryId);
                    return BadRequest("Category not found");
                }

                await _repository.CreateProductsAsync(product);
                _logger.LogInformation("****SUCCESSFULLY CREATED PRODUCT WITH ID {ProductId}****", product.Id);
                return StatusCode(201, new 
                {
                    Message = "Product created successfully.",
                    Data = product
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE CREATING THE PRODUCT****");
                return StatusCode(400, new 
                { 
                    Message = "An error occurred while creating the product",
                    Error = ex.Message 
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            try 
            {
                if (id != product.Id) 
                {
                    _logger.LogWarning("****PRODUCT ID MISMATCH. PROVIDED ID: {ProvidedId}, PRODUCT ID: {ProductId}****", id, product.Id);
                    return StatusCode(400, new 
                    {
                        Message = $"Product ID mismatch. Provided ID: {id}, Product ID: {product.Id}",
                        Data = new List<Product>() 
                    });
                }

                var existingProduct = await _repository.GetProductByIdAsync(id);
                if (existingProduct == null) 
                {
                    _logger.LogWarning("****PRODUCT WITH ID {ProductId} NOT FOUND****", id);
                    return StatusCode(404, new 
                    {
                        Message = $"Product with ID {id} not found",
                        Data = new List<Product>() 
                    });
                }

                if (!await _categoryRepository.CategoryExistsAsync(product.CategoryId))
                {
                    _logger.LogWarning("****CATEGORY WITH ID {CategoryId} NOT FOUND****", product.CategoryId);
                    return StatusCode(400, new 
                    {
                        Message = $"Category with ID {product.CategoryId} not found",
                        Data = new List<Product>() 
                    });
                }

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;

                await _repository.UpdateProductsAsync(existingProduct);
                _logger.LogInformation("****SUCCESSFULLY UPDATED PRODUCT WITH ID {ProductId}****", id);
                return StatusCode(200, new 
                {
                    Message = $"Product ID {id} updated successfully",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE UPDATING PRODUCT WITH ID {ProductId}****", id);
                return StatusCode(400, new 
                { 
                    Message = "An error occurred while updating the product",
                    Error = ex.Message 
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try 
            {
                var product = await _repository.GetProductByIdAsync(id);
                if (product == null) 
                {
                    _logger.LogWarning("****PRODUCT WITH ID {ProductId} NOT FOUND****", id);
                    return StatusCode(404, new 
                    {
                        Message = $"Product with ID {id} not found",
                        Data = new List<Product>() 
                    });
                }

                await _repository.DeleteProductsAsync(id);
                _logger.LogInformation("****SUCCESSFULLY DELETED PRODUCT WITH ID {ProductId}****", id);
                return StatusCode(200, new 
                {
                    Message = $"Product ID {id} deleted successfully.",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE DELETING PRODUCT WITH ID {ProductId}****", id);
                return StatusCode(400, new 
                { 
                    Message = "An error occurred while deleting the product",
                    Error = ex.Message 
                });
            }
        }
    }
}
