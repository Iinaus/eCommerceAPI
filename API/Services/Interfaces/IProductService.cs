using System;
using API.Models;

namespace API.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAll();

    Task<Product> GetById(int id);
}
