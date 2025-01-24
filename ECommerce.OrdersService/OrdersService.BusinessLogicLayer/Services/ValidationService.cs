using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.BusinessLogicLayer.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.BusinessLogicLayer.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task ValidateAsync<T>(T dto)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();
            if (validator is null)
                throw new InvalidOperationException($"Validator for type {typeof(T).Name} is not registered.");

            var validationResult = await validator.ValidateAsync(dto);
            if(!validationResult.IsValid)
            {
                string errors = string.Join(", ", validationResult.Errors.Select(err => err.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }
    }
}
