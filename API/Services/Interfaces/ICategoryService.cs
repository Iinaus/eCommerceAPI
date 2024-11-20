using System;
using API.Models;

namespace API.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAll();

    Task<Category> GetById(int id);

    Task<List<Product>> GetProductsByCategoryId(int id);
}
