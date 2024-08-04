using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementAppAngular.Data;
using ProductManagementAppAngular.Models;



namespace ProductManagementAppAngular.Server.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger; // Initialize logger
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                _logger.LogInformation("Received request to create product: {ProductName}", product.Name);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product: {ProductName}", product.Name);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the product." });
            }


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                _logger.LogWarning("Product ID in URL ({id}) does not match Product ID in request body ({productId})", id, product.Id);
                return BadRequest("Product ID mismatch.");
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product with ID {id} updated successfully.", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductExists(id))
                {
                    _logger.LogWarning("Product with ID {id} not found.", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error occurred while updating Product with ID {id}.", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating Product with ID {id}.", id);
                return StatusCode(500, "An unexpected error occurred.");
            }


            return NoContent();
        }

        // DELETE: api/product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}