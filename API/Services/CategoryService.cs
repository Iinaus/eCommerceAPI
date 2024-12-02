using System;
using API.Data;
using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EntityFramework;
using X.PagedList.Extensions;

namespace API.Services;

public class CategoryService(DataContext context) : ICategoryService
{
  private const int PageSize = 10;
  
  public async Task<IPagedList<Category>> GetAll(int page)
  {
    if (page < 1)
    {
      page = 1;
    }

    //TO-DO: miksi products[] on tyhjä, eli ei tuo tietoa?

    var categories = await context.Categories.ToListAsync();
    var pagedCategories = categories.ToPagedList(page, PageSize);

    return pagedCategories;
  }

  public async Task<Category> GetById(int id)
  {
    //TO-DO: miksi products[] on tyhjä, eli ei tuo tietoa?
    var category = await context.Categories.FirstAsync(category => category.Id == id);
    return category;
  }

  public async Task<IPagedList<Product>> GetProductsByCategoryId(int id, int page)
  {
    try
    {
      if (page < 1)
      {
        page = 1;
      }

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

      /*Vaihtoehtoinen tapa:
      var category = await GetById(id);
      var products = category.Products;*/

      var pagedProducts = products.ToPagedList(page, PageSize);          

      return pagedProducts;          
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
