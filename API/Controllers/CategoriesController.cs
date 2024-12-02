using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IPagedList<Category>>> GetAllCategories([FromQuery] int page = 1)
        {
            try
            {
                var categories = await service.GetAll(page);
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
        public async Task<ActionResult<List<Product>>> GetProductsByCategoryId(int id, [FromQuery] int page = 1)
        {
            try
            {
                var products = await service.GetProductsByCategoryId(id, page);
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

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<Category>> AddCategory(AddCategoryReqDto req)
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var category = await service.Create(req, loggedInUser);
                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(new ProblemDetails
                {
                    Detail = "Unable to create new category"
                });
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<Category>> UpdateCategoryById(int id, UpdateCategoryDto req)
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var category = await service.UpdateCategory(id, req);
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

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteCategoryById(int id)
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var category = await service.DeleteCategory(id);
                return Ok();
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
