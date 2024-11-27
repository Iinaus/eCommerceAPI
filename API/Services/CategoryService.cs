using System;
using API.Data;
using API.Data.Dtos;
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
            // TODO: tee DTO, jotta ei näytetä turhia tietoja?
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
            //Toinen vaihtoehto palauttaa --> missä tuon getterin on -> MODELISSA
            //return category.Products();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException();
        }

    }

    public async Task<Category> Create(AddCategoryReqDto req)
    {      
      var category = new Category
      {
        Name = req.Name,
        Description = req.Description,
        // TO-DO: tarkista, mitä tähän pitää laittaa
        UserId = 1
      };

      context.Categories.Add(category);
      await context.SaveChangesAsync();

      return category;
    }

    public async Task<Category> UpdateCategory(int id, UpdateCategoryDto req)
    {
      var category = await GetById(id);
      category.Name = req.Name;
      category.Description = req.Description;
      await context.SaveChangesAsync();
      return category;
    }

    public async Task<bool> DeleteCategory(int id)
    {
      var category = await GetById(id);
      context.Categories.Remove(category);
      await context.SaveChangesAsync();
      return true;
    }
}
