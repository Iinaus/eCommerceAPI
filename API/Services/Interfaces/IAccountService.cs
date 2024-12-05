using API.Data.Dtos;
using API.Models;

namespace API.Services.Interfaces;

public interface IAccountService
{
    Task<List<Order>> GetAll(AppUser loggedInUser);
    Task<Order> GetOrderedById(int id, AppUser loggedInUser);
    Task<bool> DeleteOrder(int id, AppUser loggedInUser);
}
