using System;
using API.Data.Dtos;
using API.Models;
using AutoMapper;

namespace API.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, UserResDto>();
    }
}
