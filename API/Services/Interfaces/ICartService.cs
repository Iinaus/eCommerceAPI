using System;
using API.Data.Dtos;
using API.Models;

namespace API.Services.Interfaces;

public interface ICartService
{
    Task<List<Order>> GetAll();
    Task<Order> AddToCart(int productId, AppUser loggedInUser);
    Task<Order> DeleteFromCart(int itemId, AppUser loggedInUser);
    Task<Order?> GetOpenOrder(AppUser loggedInUser);
    Task<Order> CreateNewOrder(AppUser loggedInUser);
}
