using System;
using API.Data.Dtos;
using API.Models;

namespace API.Services.Interfaces;

public interface IUserService
{
    Task<List<AppUser>> GetAll();

    Task<AppUser> GetById(int id);

    Task<AppUser> UpdateById(int id, UpdateUserDto req);

    Task<AppUser> Create(AddUserReqDto req);

    Task<LoginResDto?> Login(LoginReqDto req);

}
