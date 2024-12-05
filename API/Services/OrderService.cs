using System;
using API.Data;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class OrderService(DataContext context) : IOrderService
{
    public async Task<Order> Checkout(AppUser loggedInUser)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(o => o.CustomerId == loggedInUser.Id && o.State == "cart");
        
        if (order == null)
        {
            throw new InvalidOperationException("No open orders found.");
        }

        order.State = "ordered";

        await context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> Confirm(int orderId, AppUser loggedInUser)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId && o.State == "ordered");

        if (order == null)
        {
            throw new InvalidOperationException("No order to confirm was found.");
        }

        order.State = "confirmed";
        order.ConfirmedDate = DateTime.Now;
        order.HandlerId = loggedInUser.Id;

        await context.SaveChangesAsync();
        return order;
    }

}
