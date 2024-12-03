using API.Data.Dtos;
using API.Models;
using AutoMapper;

namespace API.Profiles;

public class OrderProfile : Profile
{

    public OrderProfile()
    {
        CreateMap<Order, OrderResDto>();
    }

}
