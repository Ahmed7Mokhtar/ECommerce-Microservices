using ProductsService.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductsService.DataAccessLayer.RepositoryContracts
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetAllByCondition(Expression<Func<Product, bool>> condition);
        Task<Product?> GetByCondition(Expression<Func<Product, bool>> condition);
        Task<Product?> Add(Product product);
        Task<Product?> Update(Product product);
        Task<bool> Delete(Guid id);
    }
}
