using Microsoft.AspNetCore.Http;
using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using AutoMapper;
using API.Data.Dtos;

namespace API.Controllers
{
    public class ProductsController(IProductService service, IMapper mapper) : ControllerCustomBase
    {

        [HttpGet]
        public async Task<ActionResult<IPagedList<Product>>> GetAllProducts([FromQuery] int? page)
        {
            try
            {
                var products = await service.GetAll(page);
                return Ok(
                    mapper.Map<List<ProductResDto>>(products)
                );
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
                return Ok(
                    mapper.Map<ProductResDto>(product)
                );
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    
    }
}
