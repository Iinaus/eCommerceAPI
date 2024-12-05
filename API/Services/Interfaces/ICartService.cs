using System;
using API.Data.Dtos;
using API.Models;

namespace API.Services.Interfaces;

public interface ICartService
{
    Task<Order> AddToCart(AddItemToCartReqDto req, AppUser loggedInUser);
    Task<Order> DeleteFromCart(int itemId, AppUser loggedInUser);
    Task<Order> UpdateUnitCountFromCart(int itemId, UpdateItemInCartDto req, AppUser loggedInUser);
    Task<Order?> GetOpenOrder(AppUser loggedInUser);
    Task<Order> CreateNewOrder(AppUser loggedInUser);
}
