using System;
using API.Data.Dtos;
using API.Models;

namespace API.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAll();

    Task<Category> GetById(int id);

    Task<List<Product>> GetProductsByCategoryId(int id);

    Task<Category> Create(AddCategoryReqDto req);

    Task<Category> UpdateCategory(int id, UpdateCategoryDto req);

    Task<bool> DeleteCategory(int id);
}
