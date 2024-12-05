using System;
using System.Diagnostics;
using API.Data;
using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace API.Services;

public class CategoryService(DataContext context) : ICategoryService
{
  private const int PageSize = 2;
  
  public async Task<IPagedList<Category>> GetAll(int? page)
  {
    var categories = await context.Categories
      .Include(c => c.User)
      .Include(c => c.Products)
      .ToListAsync();

    // jos sivua ei anneta, palautetaan kaikki tulokset yhdellä sivulla
    if (page < 1 || !page.HasValue)
    {
      return categories.ToPagedList(1, categories.Count);
    }

    var pagedCategories = categories.ToPagedList(page.Value, PageSize);
    return pagedCategories;
  }

  public async Task<Category> GetById(int id)
  {
    var category = await context.Categories
      .Include(c => c.User)
      .Include(c => c.Products)
      .FirstOrDefaultAsync(c => c.Id == id);

    if (category == null)
    {
      throw new InvalidOperationException("Category not found.");
    }

    return category;
  }

  public async Task<IPagedList<Product>> GetProductsByCategoryId(int id, int? page)
  {
    var category = await GetById(id);
    var products = category.Products;

    if (products.Count == 0)
    {
      throw new InvalidOperationException("Products not found.");
    }

    // jos sivua ei anneta, palautetaan kaikki tulokset yhdellä sivulla
    if (page < 1 || !page.HasValue)
    {
      return products.ToPagedList(1, products.Count);
    }

    var pagedProducts = products.ToPagedList(page.Value, PageSize);          
    return pagedProducts;          

  }

  public async Task<Category> Create(AddCategoryReqDto req, AppUser loggedInUser)
  {      
    var category = new Category
    {
      Name = req.Name,
      Description = req.Description,
      UserId = loggedInUser.Id
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
    var products = category.Products.ToList();
    
    context.Products.RemoveRange(products);
    context.Categories.Remove(category);
    
    await context.SaveChangesAsync();
    return true;
  }
}
