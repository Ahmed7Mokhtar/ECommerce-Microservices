using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using ProductsService.BusinessLogicLayer.DTOs;
using ProductsService.BusinessLogicLayer.RabbitMQ;
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
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public ProductsService(IProductsRepository productsRepository, IValidator<AddProductDTO> addProductvalidator, IValidator<UpdateProductDTO> updateProductValidator, IMapper mapper, IRabbitMQPublisher rabbitMQPublisher)
        {
            _productsRepository = productsRepository;
            _addProductvalidator = addProductvalidator;
            _updateProductValidator = updateProductValidator;
            _mapper = mapper;
            _rabbitMQPublisher = rabbitMQPublisher;
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

            var existingProduct = await _productsRepository.GetByCondition(m => m.Id == updateProductDTO.ProductId);
            if (existingProduct is null)
                throw new ArgumentException("Invalid product id");

            var product = _mapper.Map<Product>(updateProductDTO);

            var updatedProduct = await _productsRepository.Update(product);

            //string routingKey = "product.update.name";
            //var message = new ProductNameUpdateMessage(product.Id, product.Name);
            //_rabbitMQPublisher.Publish(routingKey, message);

            var headers = new Dictionary<string, object>()
            {
                { "event", "product.update" },
                { "rowCount", 1 }
            };

            var productResponse = _mapper.Map<ProductResponseDTO>(updatedProduct);

            _rabbitMQPublisher.Publish(headers, productResponse);

            return productResponse;
        }

        public async Task<bool> Delete(Guid id)
        {
            var productName = (await _productsRepository.GetByCondition(m => m.Id == id))?.Name;
            var isDeleted = await _productsRepository.Delete(id);

            if (isDeleted)
            {
                var message = new ProductDeletionMessage(id, productName);

                //string routingKey = "product.delete";
                //_rabbitMQPublisher.Publish(routingKey, message);

                var headers = new Dictionary<string, object>()
                {
                    { "event", "product.delete" },
                    { "rowCount", 1 }
                };

                _rabbitMQPublisher.Publish(headers, message);
            }

            return isDeleted;
        }
    }
}
