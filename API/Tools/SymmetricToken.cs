using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using API.Tools.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Tools;

public class SymmetricToken(IConfiguration config) : ITokenTool
{
    public string? CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"];
        if (tokenKey == null)
        {
            return null;
        }

        if (tokenKey.Length < 64)
        {
            return null;
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var _token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(7), signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(_token);

    }

}

