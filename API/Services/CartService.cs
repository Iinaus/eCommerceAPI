using System;
using API.Data;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class CartService(DataContext context) : ICartService
{
    public async Task<List<Order>> GetAll()
    {
        var orders = await context.Orders
            .Include(o => o.OrderProducts)
            .ToListAsync();
        return orders;
    }

    public async Task<Order> AddToCart(int productId, AppUser loggedInUser)
    {
        var order = await GetOpenOrder(loggedInUser);

        if (order == null) 
        {
            order = await CreateNewOrder(loggedInUser);
        }

        //TO-DO: tarkista, että productID:llä on olemassa tuote
        // ennen lisäämistä

        var addedItem = new OrderProduct
        {
            OrderId = order.Id,
            ProductId = productId,
            UnitCount = 1,
            //TO-DO: tarkista UnitPrice, minkä mukaan päivitetään
            UnitPrice = 100    
        };

        var existingItem = order.OrderProducts
            .FirstOrDefault(o => o.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.UnitCount++;
            context.OrdersProducts.Update(existingItem);
        }
        else
        {
            context.OrdersProducts.Add(addedItem);
        }

        await context.SaveChangesAsync();

        return order;        
    }

    public async Task<Order?> GetOpenOrder(AppUser loggedInUser)
    {
        var order = await context.Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.CustomerId == loggedInUser.Id && o.State == "cart");
        
        if (order == null)
        {
            order = new Order
            {
                CreatedDate = DateTime.Now,
                State = "cart",
                CustomerId = loggedInUser.Id,
                Customer = loggedInUser,
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }
        
        return order;
    }

    public async Task<Order> CreateNewOrder(AppUser loggedInUser)
    {
        var order = new Order
        {
            CreatedDate = DateTime.Now,
            State = "cart",
            CustomerId = loggedInUser.Id,
            Customer = loggedInUser,
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();
        
        return order;
    }

}
