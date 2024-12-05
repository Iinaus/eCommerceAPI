using System;
using API.Data.Dtos;
using API.Models;

namespace API.Services.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetAll();
    Task<Order> Checkout(AppUser loggedInUser);
    Task<Order> Confirm(int orderId, AppUser loggedInUser);
}