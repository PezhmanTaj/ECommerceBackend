using System.ComponentModel.DataAnnotations;
using System.Data;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSolution.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // Update
        [Authorize(Roles = "Admin, Seller")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateProductAsync(string id, [FromBody]ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _productService.UpdateProductAsync(id, product);

                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.ValidationResult.ErrorMessage });
            }
        }

        // Delete
        [Authorize(Roles = "Admin, Seller")]
        [HttpDelete ("{id}")]
        public async Task<ActionResult<bool>> DeleteProductAsync(string id)
        {
            
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
            {
                return NotFound();
            }
            return NoContent();

        }
        [Authorize(Roles = "Admin, Seller")]
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductDTO product)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _productService.CreateProductAsync(product);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.ValidationResult.ErrorMessage});
            }
        }

    }
}

