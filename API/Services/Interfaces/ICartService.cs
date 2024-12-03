using System;
using API.Data.Dtos;
using API.Models;

namespace API.Services.Interfaces;

public interface ICartService
{
    Task<Order> AddToCart(int productId, int count, AppUser loggedInUser);
    Task<Order?> CheckOpenOrders(AppUser loggedInUser);

    Task<List<Order>> GetAll();

}
