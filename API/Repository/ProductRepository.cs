using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Dtos;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;

        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public Task<List<Product>> GetTrending()
        {
            var popularProductsSinceLastWeek = _context.Order
                .Include(o => o.Items)
                .Where(o => o.OrderDate > DateTime.Now.AddDays(-7))
                .SelectMany(o => o.Items)
                .GroupBy(oi => oi.ProductId)
                .OrderBy(group => group.Sum(oi => oi.Quantity))
                .Select(g => g.Key);

            var popularProducts = _context.Product
                .Join(popularProductsSinceLastWeek, p => p.Id, popular => popular, (x, y) => x)
                .ToListAsync();

            return popularProducts;
        }

        public async Task<Product> CreateAsync(CreateProductDto productDto)
        {
            var product = productDto.ToProduct();

            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return null;

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Product.FindAsync(id);
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Product.FindAsync(id) != null;
        }

        public async Task<Product?> UpdateAsync(int id, UpdateProductDto updateDto)
        {
            var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            product.Name = updateDto.Name;
            product.Price = updateDto.Price;

            await _context.SaveChangesAsync();

            return product;
        }


    }
}