using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using ProductsService.BusinessLogicLayer.DTOs;
using ProductsService.BusinessLogicLayer.ServiceContracts;
using ProductsService.DataAccessLayer.Entities;
using ProductsService.DataAccessLayer.RepositoryContracts;
using System.Linq.Expressions;

namespace ProductsService.BusinessLogicLayer.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IValidator<AddProductDTO> _addProductvalidator;
        private readonly IValidator<UpdateProductDTO> _updateProductValidator;
        private readonly IProductsRepository _productsRepository;
        private readonly IMapper _mapper;

        public ProductsService(IProductsRepository productsRepository, IValidator<AddProductDTO> addProductvalidator, IValidator<UpdateProductDTO> updateProductValidator, IMapper mapper)
        {
            _productsRepository = productsRepository;
            _addProductvalidator = addProductvalidator;
            _updateProductValidator = updateProductValidator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<ProductResponseDTO>>(await _productsRepository.GetAll());
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllByCondition(Expression<Func<Product, bool>> condition)
        {
            return _mapper.Map<IEnumerable<ProductResponseDTO>>(await _productsRepository.GetAllByCondition(condition));
        }

        public async Task<ProductResponseDTO> GetByCondition(Expression<Func<Product, bool>> condition)
        {
            return _mapper.Map<ProductResponseDTO>(await _productsRepository.GetByCondition(condition));
        }

        public async Task<ProductResponseDTO> Add(AddProductDTO addProductDTO)
        {
            ArgumentNullException.ThrowIfNull(addProductDTO);

            // Validator addProductDTO
            ValidationResult result = await _addProductvalidator.ValidateAsync(addProductDTO);
            if (!result.IsValid)
            {
                var errorMsgs = string.Join(", ", result.Errors.Select(err => err.ErrorMessage));
                throw new ArgumentException(errorMsgs);
            }

            var product = _mapper.Map<Product>(addProductDTO);

            var addedProduct = await _productsRepository.Add(product);

            return _mapper.Map<ProductResponseDTO>(addedProduct);
        }
        public async Task<ProductResponseDTO> Update(UpdateProductDTO updateProductDTO)
        {
            ArgumentNullException.ThrowIfNull(updateProductDTO);

            // Validator updateProductDTO
            ValidationResult result = await _updateProductValidator.ValidateAsync(updateProductDTO);
            if (!result.IsValid)
            {
                var errorMsgs = string.Join(", ", result.Errors.Select(err => err.ErrorMessage));
                throw new ArgumentException(errorMsgs);
            }

            var product = _mapper.Map<Product>(updateProductDTO);

            var updatedProduct = await _productsRepository.Update(product);

            return _mapper.Map<ProductResponseDTO>(updatedProduct);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _productsRepository.Delete(id);
        }
    }
}
