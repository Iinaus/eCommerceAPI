using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrdersController(IOrderService service, IMapper mapper) : ControllerCustomBase
    {
        // Listataan kaikkien käyttäjien kaikki tilaukset (myös poistetut),
        // joten vain adminit saavat nähdä tiedot
        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            try
            {
                var orders = await service.GetAll();
                return Ok(
                    mapper.Map<List<OrderResDto>>(orders)
                );
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> HandleCheckout()
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var order = await service.Checkout(loggedInUser);
                return Ok(
                    mapper.Map<OrderResDto>(order)
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

        [HttpPost("{orderId}/confirm")]
        [Authorize(Policy = "RequireModeratorRole")]
        public async Task<ActionResult<Order>> ConfirmOrder(int orderId)
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var order = await service.Confirm(orderId, loggedInUser);
                return Ok(
                    mapper.Map<OrderResDto>(order)
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
