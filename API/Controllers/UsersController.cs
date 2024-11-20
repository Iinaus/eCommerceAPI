using API.Data;
using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetAllUsers() 
        {
            try
            {
                var users = await service.GetAll();
                return Ok(users);
            }
            catch (InvalidOperationException e)
            {
                return NotFound("users not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUserById(int id) 
        {
            try
            {
                var user = await service.GetById(id);
                return Ok(user);
            }
            catch (InvalidOperationException e)
            {
                return NotFound("user not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AppUser>> UpdateUserById(int id, UpdateUserDto req) 
        {
            try
            {
                var user = await service.UpdateById(id, req);
                return Ok(user);
            }
            catch (InvalidOperationException e)
            {
                return NotFound("user not found");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
