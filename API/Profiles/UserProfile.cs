using System;
using API.Data.Dtos;
using API.Models;
using AutoMapper;

namespace API.Profiles;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<AppUser, UserResDto>();
    }
}
