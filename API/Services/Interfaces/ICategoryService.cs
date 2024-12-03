using System;
using API.Data.Dtos;
using API.Models;
using X.PagedList;

namespace API.Services.Interfaces;

public interface ICategoryService
{
    Task<IPagedList<Category>> GetAll(int page);

    Task<Category> GetById(int id);
    Task<IPagedList<Product>> GetProductsByCategoryId(int id, int page);

    Task<Category> Create(AddCategoryReqDto req, AppUser loggedInUser);

    Task<Category> UpdateCategory(int id, UpdateCategoryDto req);

    Task<bool> DeleteCategory(int id);
}
