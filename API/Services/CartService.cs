using System;
using API.Data;
using API.Data.Dtos;
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

    public async Task<Order> AddToCart(AddItemToCartReqDto req, AppUser loggedInUser)
    {
        var product = await context.Products.FirstOrDefaultAsync(product => product.Id == req.Id);
        
        if (product == null) 
        {
            throw new InvalidOperationException("No item with given id was found");
        }
        
        var order = await GetOpenOrder(loggedInUser);

        // Jos avointa tilausta ei ole, luodaan uusi tilaus
        if (order == null) 
        {
            order = await CreateNewOrder(loggedInUser);
        }      

        // Tarkistetaan löytyykö tuote tilaukselta jo valmiiksi
        var existingItem = order.OrderProducts
            .FirstOrDefault(o => o.ProductId == req.Id);

        if (existingItem != null)
        {
            existingItem.UnitCount++;
            context.OrdersProducts.Update(existingItem);
        }
        else
        {
            var newItem = new OrderProduct
            {
                OrderId = order.Id,
                ProductId = req.Id,
                UnitCount = 1,
                UnitPrice = product.UnitPrice
            };
            context.OrdersProducts.Add(newItem);
        }

        await context.SaveChangesAsync();

        return order;        
    }

    public async Task<Order> DeleteFromCart(int itemId, AppUser loggedInUser)
    {
        var order = await GetOpenOrder(loggedInUser);

        if (order == null) 
        {
           throw new InvalidOperationException("No open order found for the user.");
        }

        var item = order.OrderProducts
            .FirstOrDefault(o => o.ProductId == itemId);

        if (item == null) 
        {
           throw new InvalidOperationException("Item not found in the order.");
        }        

        context.OrdersProducts.RemoveRange(item);

        await context.SaveChangesAsync(); 
        return order;     
    }

    public async Task<Order> UpdateUnitCountFromCart(int itemId, UpdateItemInCartDto req, AppUser loggedInUser)
    {
        var order = await GetOpenOrder(loggedInUser);

        if (order == null) 
        {
           throw new InvalidOperationException("No open order found for the user.");
        }

        var item = order.OrderProducts
            .FirstOrDefault(o => o.ProductId == itemId);

        if (item == null) 
        {
           throw new InvalidOperationException("Item not found in the order.");
        }

        // Jos tuotteen lukumäärä pudotetaan nollaan tai alle,
        // poistetaan tuote ostoskorista
        if (req.UnitCount < 1) 
        {
            await DeleteFromCart(itemId, loggedInUser);
        }  
        else 
        {
            item.UnitCount = req.UnitCount;
            await context.SaveChangesAsync(); 
        }       

        return order;     
    }

    public async Task<Order?> GetOpenOrder(AppUser loggedInUser)
    {
        var order = await context.Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.CustomerId == loggedInUser.Id && o.State == "cart");
        
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
