using System;
using API.Data.Dtos;
using API.Models;
using AutoMapper;

namespace API.Profiles;

public class OrderProductProfile : Profile
{
    public OrderProductProfile()
    {
        CreateMap<OrderProduct, OrderProductResDto>();
    }
}
