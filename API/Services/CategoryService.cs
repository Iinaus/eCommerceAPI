using System;
using System.Diagnostics;
using API.Data;
using API.Data.Dtos;
using API.Models;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EntityFramework;
using X.PagedList.Extensions;

namespace API.Services;

public class CategoryService(DataContext context, IMapper mapper) : ICategoryService
{
  private const int PageSize = 10;
  
  public async Task<IPagedList<CategoryResDto>> GetAll(int page)
  {
    if (page < 1)
    {
      page = 1;
    }

    var categories = await context.Categories
      .Include(c => c.User)
      .Include(c => c.Products)
      .ToListAsync();

    var categoryDtos = mapper.Map<List<CategoryResDto>>(categories);

    var pagedCategories = categoryDtos.ToPagedList(page, PageSize);

    return pagedCategories;
  }

  public async Task<Category> GetById(int id)
  {
    var category = await context.Categories
      .Include(c => c.User)
      .Include(c => c.Products)
      .FirstAsync(c => c.Id == id);

    return category;
  }

  public async Task<IPagedList<ProductResDto>> GetProductsByCategoryId(int id, int page)
  {
    try
    {
      if (page < 1)
      {
        page = 1;
      }

      var category = await GetById(id);
      var products = category.Products;
      var productsDto = mapper.Map<List<ProductResDto>>(products);

      var pagedProducts = productsDto.ToPagedList(page, PageSize);          

      return pagedProducts;          
    }
    catch (InvalidOperationException)
    {
        throw new InvalidOperationException();
    }
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
