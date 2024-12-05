using API.Data;
using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController(IUserService service, IMapper mapper) : ControllerCustomBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetAllUsers() 
        {
            try
            {
                var users = await service.GetAll();
                return Ok(
                    mapper.Map<List<UserResDto>>(users)
                );
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
                return Ok(
                    mapper.Map<UserResDto>(user)
                );
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
                return Ok(
                    mapper.Map<UserResDto>(user)
                );
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

        [HttpPost("register")]
        public async Task<ActionResult<UserResDto>> Register(AddUserReqDto req)
        {
            try
            {
                var user = await service.Create(req);
                return Ok(
                    mapper.Map<UserResDto>(user)
                );
            }
            catch (Exception e)
            {
                return BadRequest(new ProblemDetails
                {
                    Detail = "Unable to create new user"
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResDto>> Login(LoginReqDto req)
        {
            try
            {
                var token = await service.Login(req);
                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
