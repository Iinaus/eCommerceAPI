using System;
using API.Data.Dtos;
using API.Models;

namespace API.Services.Interfaces;

public interface IUserService
{
    Task<List<AppUser>> GetAll();

    Task<AppUser> GetById(int id);

    Task<AppUser> UpdateRoleById(int id, UpdateUserRoleReqDto req);

    Task<AppUser> Create(AddUserReqDto req);

    Task<LoginResDto?> Login(LoginReqDto req);

}
