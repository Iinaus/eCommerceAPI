using System;
using API.Data;
using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class OrderService(DataContext context) : IOrderService
{
    public async Task<Order> Checkout(AppUser loggedInUser)
    {
        var order = await context.Orders
            .FirstAsync(o => o.CustomerId == loggedInUser.Id && o.State == "cart");

        order.State = "ordered";

        await context.SaveChangesAsync();
        return order;
    }

}
