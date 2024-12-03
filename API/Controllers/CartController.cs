using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService service, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            try
            {
                var orders = await service.GetAll();
                return Ok(orders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("/items")]
        [Authorize]
        public async Task<ActionResult<Order>> AddItemToCart(int productId, int count) 
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var order = await service.AddToCart(productId, count, loggedInUser);
                return Ok(order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
