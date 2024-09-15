using Microsoft.AspNetCore.Mvc;
using StoreAPI.Models;
using StoreAPI.Repositories;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<WishlistsController> _logger;

        public WishlistsController(
            IWishlistRepository wishlistRepository,
            IProductRepository productRepository,
            ILogger<WishlistsController> logger
        )
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        
        [HttpGet("{name}")]
        public async Task<IActionResult> GetWishlistByName(string name)
        {
            try
            {
                var wishlist = await _wishlistRepository.GetWishlistByNameAsync(name);

                if (wishlist == null)
                {
                    _logger.LogWarning("****WISHLIST NOT FOUND****");
                    return NotFound(new 
                    {
                        Message = $"Wishlist with name '{name}' not found."
                    });
                }

                _logger.LogInformation("****SUCCESSFULLY RETRIEVED WISHLIST****");


                return Ok(new 
                {
                    Message = "Successfully retrieved the wishlist",
                    Data = wishlist
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE GETTING WISHLIST****");
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while getting the wishlist",
                    Error = ex.Message 
                });
            }
}


        [HttpGet]
        public async Task<IActionResult> GetAllWishlists()
        {
            try 
            {
                var wishlists = await _wishlistRepository.GetAllWishListAsync();

                if (wishlists == null || !wishlists.Any())
                {
                    _logger.LogWarning("****NO WISHLISTS FOUND****");
                    return StatusCode(400, new 
                    {
                        Message = "No wishlists found",
                        Data = new List<Wishlist>() 
                    });
                }

                _logger.LogInformation("****SUCCESSFULLY RETRIEVED ALL WISHLISTS****");
                return StatusCode(200, new 
                {
                    Message = "Successfully retrieved all wishlists",
                    Data = wishlists 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "****AN ERROR OCCURRED WHILE GETTING ALL WISHLISTS****");
                return StatusCode(400, new 
                { 
                    Message = "An error occurred while getting all wishlists",
                    Error = ex.Message 
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateWishlist([FromBody] Wishlist wishlist)
        {
            try
            {
                if (wishlist == null || string.IsNullOrWhiteSpace(wishlist.Name))
                {
                    return BadRequest("Wishlist name is required.");
                }

                var existingWishlist = await _wishlistRepository.GetWishlistByNameAsync(wishlist.Name);
                if (existingWishlist != null)
                {
                    return BadRequest("Wishlist with this name already exists.");
                }

                await _wishlistRepository.CreateWishlistAsync(wishlist);
                _logger.LogInformation("Wishlist created successfully.");
                return StatusCode(201, new { Message = "Wishlist created successfully.", Data = wishlist });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating wishlist.");
                return StatusCode(500, new { Message = "Error creating wishlist.", Error = ex.Message });
            }
        }

        [HttpPost("{wishlistName}/products/{productId}")]
        public async Task<ActionResult> AddProductToWishlist(string wishlistName, int productId)
        {
            try
            {
                var wishlist = await _wishlistRepository.GetWishlistByNameAsync(wishlistName);
                if (wishlist == null)
                {
                    return NotFound($"Wishlist '{wishlistName}' not found.");
                }

                var product = await _productRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    return NotFound($"Product with ID {productId} not found.");
                }

                if (await _wishlistRepository.IsProductInWishlistAsync(wishlist.Id, productId))
                {
                    return BadRequest("Product is already in the wishlist.");
                }

                await _wishlistRepository.AddProductToWishlistAsync(wishlist, product);
                _logger.LogInformation("Product added to wishlist successfully.");
                return Ok(new { Message = "Product added to wishlist successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product to wishlist.");
                return StatusCode(500, new { Message = "Error adding product to wishlist.", Error = ex.Message });
            }
        }


        [HttpDelete("{wishlistName}/products/{productId}")]
        public async Task<ActionResult> RemoveProductFromWishlist(string wishlistName, int productId)
        {
            try
            {
                var wishlist = await _wishlistRepository.GetWishlistByNameAsync(wishlistName);
                if (wishlist == null)
                {
                    return NotFound($"Wishlist '{wishlistName}' not found.");
                }

                await _wishlistRepository.RemoveProductFromWishlistAsync(wishlist, productId);
                _logger.LogInformation("Product removed from wishlist successfully.");
                return Ok(new { Message = "Product removed from wishlist successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing product from wishlist.");
                return StatusCode(500, new { Message = "Error removing product from wishlist.", Error = ex.Message });
            }
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteWishlist(string name)
        {
            try
            {
                await _wishlistRepository.DeleteWishlistByNameAsync(name);
                _logger.LogInformation("Wishlist deleted successfully.");
                return Ok(new { Message = "Wishlist deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting wishlist.");
                return StatusCode(400, new 
                {
                    Message = "Error deleting wishlist.",
                    Error = ex.Message 
                });
            }
        }
    }
}
