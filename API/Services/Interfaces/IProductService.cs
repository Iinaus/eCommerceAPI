using System;
using API.Data.Dtos;
using API.Models;
using X.PagedList;

namespace API.Services.Interfaces;

public interface IProductService
{
    Task<IPagedList<Product>> GetAll(int? page);

    Task<Product> GetById(int id);
}
