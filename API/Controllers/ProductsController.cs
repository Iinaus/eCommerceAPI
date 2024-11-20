using Microsoft.AspNetCore.Http;
using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts() 
        {
            try
            {
                var products = await service.GetAll();
                return Ok(products);
            }
            catch (InvalidOperationException e)
            {
                return NotFound("products not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id) 
        {
            try
            {
                var product = await service.GetById(id);
                return Ok(product);
            }
            catch (InvalidOperationException e)
            {
                return NotFound("product not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    
    }
}
