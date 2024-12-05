using System;
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

public class ProductService(DataContext context) : IProductService
{
   public async Task<IPagedList<Product>> GetAll(int? page, int pageSize)
   {
      var products = await context.Products.ToListAsync();

      // jos sivua ei anneta, palautetaan kaikki tulokset yhdellä sivulla
      if (page < 1 || !page.HasValue)
      {
         return products.ToPagedList(1, products.Count);
      }

      // lähde 1) ChatGPT, katso tarkempi kuvaus tiedoston lopusta
      var pagedProducts = products.ToPagedList(page.Value, pageSize);

      return pagedProducts;
   }

   public async Task<Product> GetById(int id)
   {
      var product = await context.Products.FirstOrDefaultAsync(product => product.Id == id);

      if (product == null)
      {
         throw new InvalidOperationException("Product not found.");
      }
      return product;
   }
  
}

/* LÄHTEET
lähde 1) chatGPT
-------------------
Kysymys 1: Easiest way to apply paging in .NET Core

Vastaus:
Paging in .NET Core can be implemented efficiently using LINQ and a library 
like X.PagedList, or by manual implementation using skip/take with queryable 
collections. Here's a straightforward guide:

1. Manual Paging with LINQ
This method involves using Skip() and Take() with your dataset.
2. Using X.PagedList
X.PagedList is a popular NuGet package to simplify pagination.

Kysymys 2: Give an example of using X.PagedList and how to install

Vastaus:
To use X.PagedList in your .NET Core application for paging, 
you'll first need to install the X.PagedList NuGet package, 
and then use it in your application for paging data efficiently.

Step 1: Installing X.PagedList. Using .NET CLI:

If you are using the .NET CLI, run this command in your project directory:
dotnet add package X.PagedList

Step 2: Implementing Paging with X.PagedList
Here’s an example of how you can implement paging with X.PagedList in a typical .NET Core Web API controller.
1. Create the Item Entity (if you don't already have one)
2. Install the X.PagedList NuGet Package as shown above.
3: Create the Service Class
The ItemService class handles business logic, such as fetching the data from the
database and applying pagination.

using X.PagedList;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class ItemService
{
    // Fetch data and apply pagination
    public async Task<IPagedList<Item>> GetPagedItems(int pageNumber = 1, int pageSize = 10)
    {
        return await _context.Items
            .OrderBy(i => i.Name)  // Optional: Order by Name
            .ToPagedListAsync(pageNumber, pageSize);  // Pagination using X.PagedList
    }
}

4: Create the Controller Class
The ItemsController uses the ItemService to fetch the data and return it through
the API.

 [HttpGet]
    public async Task<IActionResult> GetItems(int pageNumber = 1, int pageSize = 10)
    {
        var items = await _itemService.GetPagedItems(pageNumber, pageSize);

        // Return the paginated list as JSON
        return Ok(items);
    }
*/
