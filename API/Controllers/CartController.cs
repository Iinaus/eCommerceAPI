using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController(ICartService service, IMapper mapper) : ControllerCustomBase
    {
        [HttpGet]
        //[Authorize(Policy = "RequireAdminRole")]
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


        [HttpPost("items")]
        [Authorize]
        public async Task<ActionResult<Order>> AddItemToCart(AddItemToCartReqDto req) 
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var order = await service.AddToCart(req, loggedInUser);
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

        [HttpDelete("items/{itemId}")]
        [Authorize]
        public async Task<ActionResult<Order>> DeleteItemFromCart(int itemId) 
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var order = await service.DeleteFromCart(itemId, loggedInUser);
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

        [HttpPatch("items/{itemId}")]
        [Authorize]
        public async Task<ActionResult<Order>> UpdateItemsUnitCountFromCart(int itemId, UpdateItemInCartDto req) 
        {
            try
            {
                if (HttpContext.Items["loggedInUser"] is not AppUser loggedInUser)
                {
                    return Unauthorized();
                }

                var order = await service.UpdateUnitCountFromCart(itemId, req, loggedInUser);
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
