using System;
using API.Data;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class CategoryService(DataContext context) : ICategoryService
{
    public async Task<List<Category>> GetAll()
    {
        var categories = await context.Categories.ToListAsync();
        return categories;
    }

    public async Task<Category> GetById(int id)
    {
       var category = await context.Categories.FirstAsync(category => category.Id == id);
       return category;
    }

    public async Task<List<Product>> GetProductsByCategoryId(int id)
    {
        /* TODO: jos tarkistetaan, niin näytetään vain tuotteet, joiden
        kategoria on tietokannassa omassa taulussaan. JOS on tuotteita, joiden
        kategoriaa ei ole omassa taulussa, ne eivät nouse - ONKO OK?
        */
        try
        {
            var category = await GetById(id);
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException();
        }

        var products = await context.Products
            .Where(p => p.CategoryId == id)
            .Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                UnitPrice = p.UnitPrice
            })
            .ToListAsync();

        return products;
    }
}
