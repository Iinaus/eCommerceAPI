using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController(IAccountService service, IMapper mapper) : ControllerCustomBase
    {
        [HttpGet("orders")]
        [Authorize]
        public async Task<ActionResult> GetAllOrders()
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var orders = await service.GetAll(loggedInUser);
                return Ok(
                    mapper.Map<List<OrderResDto>>(orders)
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

        [HttpDelete("orders/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var order = await service.DeleteOrder(id, loggedInUser);
                return Ok();
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
