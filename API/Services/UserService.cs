using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Data.Dtos;
using API.Data.Enums;
using API.Models;
using API.Services.Interfaces;
using API.Tools.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace API.Services;

public class UserService(DataContext context, ITokenTool tokenCreator) : IUserService
{
   public async Task<List<AppUser>> GetAll()
   {
      var users = await context.Users.ToListAsync();
      return users;
   }

   public async Task<AppUser> GetById(int id)
   {
      var user = await context.Users.FirstOrDefaultAsync(user => user.Id == id);
      
      if (user == null)
      {
         throw new InvalidOperationException("User not found.");
      }

      return user;
   }
  
   public async Task<AppUser> UpdateRoleById(int id, UpdateUserRoleReqDto req)
   {
      // lähde 1) ChatGPT, katso tarkempi kuvaus tiedoston lopusta
      if (!Enum.TryParse(typeof(UserRole), req.Role, true, out var parsedRole))
      {
         throw new Exception($"Invalid role. Role must be one of the following: {string.Join(", ", Enum.GetNames(typeof(UserRole)))}");
      }

      var user = await GetById(id);

      user.Role = req.Role;
      
      await context.SaveChangesAsync();
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

/*
lähde 1) chatGPT
-------------------
Kysymys: I am using .net core. How can I check that varbiale is in enum?

Vastaus: 
In .NET Core, you can check if a variable is a valid value of an enum using the
Enum.IsDefined method. Here's how you can do it:

namespace EnumCheckExample
{
    public enum Colors
    {
        Red,
        Green,
        Blue
    }

    class Program
    {
        static void Main(string[] args)
        {
            object variable = "Red"; // This can be any type

            // Check if variable is in Colors enum
            bool isInEnum = Enum.TryParse(typeof(Colors), variable.ToString(), true, out var parsedValue)
                            && Enum.IsDefined(typeof(Colors), parsedValue);

            Console.WriteLine($"Is '{variable}' in Colors enum? {isInEnum}");
        }
    }
}
*/

