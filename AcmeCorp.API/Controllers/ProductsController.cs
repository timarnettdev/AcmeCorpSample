using AcmeCore.Infrastructure.Models;
using AcmeCore.Service.Services.Interfaces;
using AcmeCorp.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace AcmeCorp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAllProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductViewModel>> GetProductByIdAsync(long productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> CreateProductAsync(ProductViewModel productViewModel)
        {
            var createdProduct = await _productService.CreateProductAsync(productViewModel);
            return CreatedAtAction(nameof(GetProductByIdAsync), new { productId = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProductAsync(long productId, ProductViewModel productViewModel)
        {
            if (productId != productViewModel.Id)
            {
                return BadRequest();
            }

            try
            {
                await _productService.UpdateProductAsync(productViewModel);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductAsync(long productId)
        {
            try
            {
                await _productService.DeleteProductAsync(productId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
