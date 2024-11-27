using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using API.Tools.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace API.Services;

public class UserService(DataContext context, ITokenTool tokenCreator) : IUserService
{
   public async Task<List<AppUser>> GetAll()
   {
      var users = await context.Users.ToListAsync();
      //TO-DO: ei saa palauttaa koko useria: näkyy salasanat yms.
      // Pitäisi palauttaa vain ne tiedot, jotka saa näyttää
      return users;
   }

   public async Task<AppUser> GetById(int id)
   {
      var user = await context.Users.FirstAsync(user => user.Id == id);
      //TO-DO: ei saa palauttaa koko useria: näkyy salasanat yms.
      // Pitäisi palauttaa vain ne tiedot, jotka saa näyttää
      return user;
   }
  
   public async Task<AppUser> UpdateById(int id, UpdateUserDto req)
   {
      var user = await GetById(id);
      user.UserName = req.UserName;
      await context.SaveChangesAsync();
      //TO-DO: ei saa palauttaa koko useria: näkyy salasanat yms.
      // Pitäisi palauttaa vain ne tiedot, jotka saa näyttää
      return user;
   }

   public async Task<AppUser> Create(AddUserReqDto req)
   {
      using var hmac = new HMACSHA512();
      
      var user = new AppUser
      {
         UserName = req.UserName,
         Role = "customer",
         PasswordSalt = hmac.Key,
         HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(req.Password))
      };

      context.Users.Add(user);
      await context.SaveChangesAsync();

      return user;
   }

   public async Task<LoginResDto?> Login(LoginReqDto req)
   {
      var user = await context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == req.UserName.ToLower());

      if (user == null)
      { 
         return null;
      }

      using var hmac = new HMACSHA512(user.PasswordSalt);

      var computedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(req.Password));
      for (int i = 0; i < computedPassword.Length; i++)
      {
         if(computedPassword[i] != user.HashedPassword[i])
         { 
            return null;
         }
      }

      var token = tokenCreator.CreateToken(user);
      if (token == null) 
      {
         return null;
      }

      return new LoginResDto
      {
         Token = token
      };

   } 
}

