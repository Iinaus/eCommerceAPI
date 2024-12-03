using System;
using System.Data.Entity;
using API.Data;
using API.Models;
using API.Services.Interfaces;

namespace API.Services;

public class CartService(DataContext context) : ICartService
{
    public async Task<List<Order>> GetAll()
    {
        var orders = await context.Orders.ToListAsync();
        return orders;
    }

    public async Task<Order> AddToCart(int productId, int count, AppUser loggedInUser)
    {
        Console.WriteLine("########################################");
        Console.WriteLine("AddToCart rivi 14");
        var order = await CheckOpenOrders(loggedInUser);
        Console.WriteLine("########################################");
        Console.WriteLine("AddToCart rivi 17");

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
        }

        var orderedProducts = new OrderProduct
        {
            OrderId = order.Id,
            ProductId = productId,
            UnitCount = count,
            //TO-DO: tarkista UnitPrice, minkä mukaan päivitetään
            UnitPrice = 100,
            Order = order      
        };

        context.OrdersProducts.Add(orderedProducts);        
        await context.SaveChangesAsync();

        return order;
    }

    public async Task<Order?> CheckOpenOrders(AppUser loggedInUser)
    {
        Console.WriteLine("########################################");
        Console.WriteLine("CheckOpenOrders rivi 46");

        var order = await context.Orders
            .Include(order => order.OrderProducts)
            .FirstOrDefaultAsync(order => order.CustomerId == loggedInUser.Id && order.State == "cart");
        
        Console.WriteLine("########################################");
        Console.WriteLine("CheckOpenOrders rivi 56");

        return order;
    }

}
