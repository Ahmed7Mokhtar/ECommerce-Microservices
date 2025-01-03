using Microsoft.EntityFrameworkCore;
using ProductsService.DataAccessLayer.Context;
using ProductsService.DataAccessLayer.Entities;
using ProductsService.DataAccessLayer.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductsService.DataAccessLayer.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _context;

        public ProductsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByCondition(Expression<Func<Product, bool>> condition)
        {
            return await _context.Products.Where(condition).ToListAsync();
        }

        public async Task<Product?> GetByCondition(Expression<Func<Product, bool>> condition)
        {
            return await _context.Products.FirstOrDefaultAsync(condition);
        }

        public async Task<Product?> Add(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> Delete(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if(product is not null)
            {
                _context.Products.Remove(product);
                int rowsAffected = await _context.SaveChangesAsync();
                return rowsAffected > 0 ? true : false;
            }

            return false;
        }

        public async Task<Product?> Update(Product product)
        {
            var isProductExist = await _context.Products.AnyAsync(m => m.Id == product.Id);
            if (isProductExist)
            {
                _context.Entry(product).Property(m => m.Name).IsModified = true;
                _context.Entry(product).Property(m => m.Category).IsModified = true;
                _context.Entry(product).Property(m => m.UnitPrice).IsModified = true;
                _context.Entry(product).Property(m => m.QuantityInStock).IsModified = true;

                await _context.SaveChangesAsync();

                return product;
            }

            return null;
        }
    }
}
