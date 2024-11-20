using System;
using API.Data;
using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class UserService(DataContext context) : IUserService
{
    public async Task<List<AppUser>> GetAll()
    {
       var users = await context.Users.ToListAsync();
       return users;
    }

    public async Task<AppUser> GetById(int id)
    {
       var user = await context.Users.FirstAsync(user => user.Id == id);
       return user;
    }
  
    public async Task<AppUser> UpdateById(int id, UpdateUserDto req)
    {
       var user = await GetById(id);
       user.UserName = req.UserName;
       await context.SaveChangesAsync();
       return user;
    }
}

