using ProductsService.BusinessLogicLayer.DTOs;
using ProductsService.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductsService.BusinessLogicLayer.ServiceContracts
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAll();
        Task<IEnumerable<ProductResponseDTO>> GetAllByCondition(Expression<Func<Product, bool>> condition);
        Task<ProductResponseDTO> GetByCondition(Expression<Func<Product, bool>> condition);
        Task<ProductResponseDTO> Add(AddProductDTO addProductDTO);
        Task<ProductResponseDTO> Update(UpdateProductDTO updateProductDTO);
        Task<bool> Delete(Guid id);
    }
}
