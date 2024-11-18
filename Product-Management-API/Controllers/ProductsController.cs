using Microsoft.AspNetCore.Mvc;
using ProductManagementServices.Services;
using System.Net;
using ProductManagementModel.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Product_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductMamangementService _productService;

        public ProductsController(IProductMamangementService productService)
        {
            this._productService = productService;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get(string? searchItem, int _page, int _limit)
        {
            try {
               // List<Product> filterdata = null;
                var response = await this._productService.GetAllProducts();
                if (response == null )
                    return NotFound();
                if (response != null)
                {
                   
                    

                    if (!String.IsNullOrEmpty(searchItem))
                    {
                        var filteredProducts = response
                        .Where(p => p.Name.Contains(searchItem, StringComparison.OrdinalIgnoreCase))
                        .Skip((_page - 1) * _limit)
                        .Take(_limit)
                        .ToList();

                        return Ok(filteredProducts);
                    }
                    else
                    {
                        var filteredProducts = response
                         .Skip((_page - 1) * _limit)
                         .Take(_limit)
                         .ToList();

                        return Ok(filteredProducts);
                    }
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "Issue in API!");
                }

            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Exception occured in API: "+ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            try
            {
                if (product == null) { return BadRequest(); }
               
                await _productService.AddProduct(product);
                return Ok();

            }
            catch (Exception ex) {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Exception occured in API: " + ex.Message);
            }
            
            
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post(int id, [FromBody] Product product)
        {
            try
            {
                if (id != product.Id) return BadRequest();
                await _productService.UpdateProduct(product);
                return Ok();
            }
            catch (Exception ex) {
                return StatusCode((int) HttpStatusCode.InternalServerError, "Exception occured in API: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0) return BadRequest();
                await _productService.DeleteProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Exception occured in API: " + ex.Message);
            }

        }
    }
}
