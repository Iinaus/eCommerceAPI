using System;

namespace API.Data.Dtos;

public class UserResDto
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Role { get; set; }
}