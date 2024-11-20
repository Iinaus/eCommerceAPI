using System;
using API.Data;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class ProductService(DataContext context) : IProductService
{
    public async Task<List<Product>> GetAll()
    {
       var products = await context.Products.ToListAsync();
       return products;
    }

    public async Task<Product> GetById(int id)
    {
       var product = await context.Products.FirstAsync(product => product.Id == id);
       return product;
    }
  
}
