using API.Data.Dtos;
using API.Models;
using AutoMapper;

namespace API.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResDto>();
    }

}
