using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Repository
{
    public interface IProductRepository : IGenericRepository<Product, CreateProductDto, UpdateProductDto>
    {
        Task<List<Product>> GetTrending();
    }
}