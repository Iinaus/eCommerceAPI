using System;
using API.Models;

namespace API.Tools.Interfaces;

public interface ITokenTool
{
    string? CreateToken(AppUser user);
}
