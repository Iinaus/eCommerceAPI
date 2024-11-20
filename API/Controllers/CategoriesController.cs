using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories() 
        {
            try
            {
                var categories = await service.GetAll();
                return Ok(categories);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id) 
        {
            try
            {
                var category = await service.GetById(id);
                return Ok(category);
            }
            catch (InvalidOperationException e)
            {
                return NotFound("category not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategoryId(int id)
        {
            try
            {
                var products = await service.GetProductsByCategoryId(id);
                return Ok(products);
            }
            catch (InvalidOperationException e)
            {
                return NotFound("category not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
