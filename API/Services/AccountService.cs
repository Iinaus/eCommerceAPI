using System;
using API.Data;
using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class AccountService(DataContext context) : IAccountService
{
    public async Task<List<Order>> GetAll(AppUser loggedInUser)
    {
        var orders = await context.Orders
            .Include(o => o.OrderProducts)
            .Where(o => o.CustomerId == loggedInUser.Id)
            .ToListAsync();
        
        if (orders == null)
        {
            throw new InvalidOperationException("No orders found.");
        }

        return orders;
    }

    public async Task<Order> GetOrderedById(int id, AppUser loggedInUser)
    {
        var order = await context.Orders
            .Include(o => o.OrderProducts)
            .Where(o => o.Id == id && o.CustomerId == loggedInUser.Id && o.State == "ordered")
            .FirstOrDefaultAsync();

        if (order == null) 
        {
           throw new InvalidOperationException("No order found");
        }

        return order;
    }

    public async Task<bool> DeleteOrder(int id, AppUser loggedInUser)
    {
        var order = await GetOrderedById(id, loggedInUser);

        order.RemovedDate = DateTime.Now;
        order.State = "cancelled";

        await context.SaveChangesAsync();

        return true;       
    }
}
